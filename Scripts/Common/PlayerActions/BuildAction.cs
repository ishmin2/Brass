using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.DefaultBuilding;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.ActionBoard.Events;
using Assets.Scripts.Client.GameFolder.ApprovePanel.Events;
using Assets.Scripts.Client.GameFolder.Dashboard.Events;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket.Events;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Common.Pathfind;
using Assets.Scripts.Common.PlayerActions.Interfaces;

// TODO: Если в городе есть выбор между полной клеткой предприятия и 1/2, нужно сначала выбирать полную
namespace Assets.Scripts.Common.PlayerActions
{
    public class BuildAction : IPlayerAction
    {
        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; } = new Dictionary<ISnapshotable, ISnapshot>();

        private readonly SteelMarket steelMarket;
        private readonly CoalMarket coalMarket;
        private readonly UIManager uiManager;
        private readonly MoneyManager moneyManager;
        private readonly MusicManager musicManager;
        private readonly MapGraph mapGraph;
        private readonly PlayerCard card;

        private Building currentBuilding;
        private bool canFinishAction;
        private List<Building> availableForBuild;
        private List<Building> availableForRebuild;
        private List<Building> availableForUpgrade;

        public BuildAction(SteelMarket steelMarket, CoalMarket coalMarket, UIManager uiManager,
            MoneyManager moneyManager, MusicManager musicManager, PlayerCard card)
        {
            this.steelMarket = steelMarket;
            this.coalMarket = coalMarket;
            this.uiManager = uiManager;
            this.moneyManager = moneyManager;
            this.musicManager = musicManager;
            this.card = card;

            Snapshotables.Add(this.moneyManager, this.moneyManager.Save());
            Snapshotables.Add(this.steelMarket, this.steelMarket.Save());
            Snapshotables.Add(this.coalMarket, this.coalMarket.Save());
            Snapshotables.Add(uiManager.BuildingsDashboard, uiManager.BuildingsDashboard.Save());
            foreach (var building in this.mapGraph.GetBuildings())
            {
                Snapshotables.Add(building, building.Save());
            }
            ClientEventAggregator.Subscribe(this);
        }

        public bool CanStartAction(ref string reason)
        {
            bool hasError = false;
            if (this.uiManager.BuildingsDashboard.columns.SelectMany(c => c.Buttons).All(b => b.DashboardButtonCounter.IsEmpty()))
            {
                reason = "No buildings available";
                hasError = true;
            }
            else if (this.card.City != CityNames.None)
            {
                var city = this.mapGraph.GetCityByName(this.card.City);
                this.SetBuildingVariables(new[] { city }, new[] { BuildingType.CoalMine, BuildingType.SteelFactory, BuildingType.Harbor, BuildingType.Shipyard, BuildingType.CottonFactory });
            }
            else if (this.card.Type != BuildingType.None)
            {
                this.SetBuildingVariables(this.mapGraph.GetCities(), new[] { this.card.Type });
            }

            if (!this.availableForRebuild.Any() && !this.availableForUpgrade.Any() && !this.availableForBuild.Any())
            {
                reason = "No available actions in chosen city";
                hasError = true;
            }

            if (hasError)
            {
                ClientEventAggregator.Publish(new CancelActionActionEvent());
            }

            return !hasError;
        }

        public void StartAction()
        {
            this.SetBuildingStates();
        }

        public void FinishAction()
        {
            if (Container.Instance().GetPlayerPanel().LocalMoney > 0 && this.canFinishAction)
            {
                this.musicManager.Play(SoundConstants.Yes);

                ClientEventAggregator.Publish(new EndActionEvent());
                ClientEventAggregator.Unsubscribe(this);
            }
            else
            {
                if (Container.Instance().GetPlayerPanel().LocalMoney < 0)
                {
                    ClientEventAggregator.Publish(new ValidationErrorActionEvent("Not enough money to finish building"));
                }
                else if (!this.canFinishAction)
                {
                    ClientEventAggregator.Publish(new ValidationErrorActionEvent("Need to buy required resources"));
                }
            }
        }

        public void CancelAction()
        {
            
        }

        public async void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is BuildingToConstructChosenEvent buildingSpotChosenEvent)
            {
                this.currentBuilding = buildingSpotChosenEvent.Building;
                this.DisableBuildTargets();
                this.EnableDashboardForBuildSpot(buildingSpotChosenEvent.Building.AvailableBuildingType);
            }

            if (eventType is UpgradeBuildingChosenActionEvent upgradeBuildingChosenEvent)
            {
                this.currentBuilding = upgradeBuildingChosenEvent.Building;
                this.DisableBuildTargets();
                this.EnableDashboardForRebuild(upgradeBuildingChosenEvent.Building.BuildingType, upgradeBuildingChosenEvent.Building.Level);
            }

            if (eventType is DashboardBuildingChosenActionEvent dashboardBuildingChosenEvent)
            {
                this.uiManager.DisableBuildingDashboard();
                this.moneyManager.ChangeMoney(-dashboardBuildingChosenEvent.Building.price.MoneyCost);
                //ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = Container.i().GetPlayerPanel().LocalMoney });
                // this.currentBuilding.UpdateBuilding(dashboardBuildingChosenEvent.Building, Container.GetLocalPlayer().PlayerColor);

                if (!await this.FinishBuild())
                {
                    this.currentBuilding.SetGhostState();
                    this.EnableResourcesToBuy(this.currentBuilding);
                }
            }

            if (eventType is BuildingSellResourceRequestModel buildingSellResourceRequestModel)
            {
                var building = this.mapGraph.GetCityByName(buildingSellResourceRequestModel.City).Buildings().Single(b =>
                    b.CitySlotNumber == buildingSellResourceRequestModel.BuildingSlot);

                if (building.BuildingType == BuildingType.CoalMine)
                {
                    this.currentBuilding.price.CoalToBuildRemind--;
                    building.SetIdleState();
                    await this.FinishBuild();
                }
                else if (building.BuildingType == BuildingType.SteelFactory)
                {
                    this.currentBuilding.price.SteelToBuildRemind--;
                    building.SetIdleState();
                    await this.FinishBuild();
                }
            }

            if (eventType is MarketBuyResourceRequestModel coalSellEvent && coalSellEvent.MarketType == MarketType.CoalMarket)
            {
                this.currentBuilding.price.CoalToBuildRemind--;
                ClientEventAggregator.Publish(new DisableCoalMarketActionEvent());

                this.moneyManager.ChangeMoney(-coalSellEvent.Cost);
                // ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = Container.i().GetPlayerPanel().LocalMoney });
                var coalBuildings = this.mapGraph.GetBuildingsByType(BuildingType.CoalMine).Where(s => s.CoalToSellRemind > 0);
                foreach (var coalBuilding in coalBuildings)
                {
                    coalBuilding.SetIdleState();
                }

                await this.FinishBuild();
            }

            if (eventType is MarketBuyResourceRequestModel steelSellEvent && steelSellEvent.MarketType == MarketType.SteelMarket)
            {
                this.currentBuilding.price.SteelToBuildRemind--;
                ClientEventAggregator.Publish(new DisableSteelMarketActionEvent());

                this.moneyManager.ChangeMoney(-steelSellEvent.Cost);
                // ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = Container.i().GetPlayerPanel().LocalMoney });
                var steelBuildings = this.mapGraph.GetBuildingsByType(BuildingType.SteelFactory).Where(s => s.SteelToSellRemind > 0);
                foreach (var steelBuilding in steelBuildings)
                {
                    steelBuilding.SetIdleState();
                }

                await this.FinishBuild();
            }
        }

        private async Task<bool> FinishBuild()
        {
            var canBuild = this.currentBuilding.price.CoalToBuildRemind == 0 && this.currentBuilding.price.SteelToBuildRemind == 0;
            if (canBuild)
            {
                ClientEventAggregator.Publish(new BuildingConstructedRequestModel
                {
                    BuildingLevel = this.currentBuilding.Level,
                    BuildingSlot = this.currentBuilding.CitySlotNumber,
                    BuildingType = this.currentBuilding.BuildingType,
                    City = this.currentBuilding.GetCity().Name,
                    CoalToSellRemind = this.currentBuilding.CoalToSellRemind,
                    SteelToSellRemind = this.currentBuilding.SteelToSellRemind,
                    // PlayerColor = Container.GetLocalPlayer().PlayerColor,
                });

                await this.currentBuilding.SetFinishStateAsync();
                this.canFinishAction = true;
                return true;
            }

            return false;
        }

        private void EnableDashboardForBuildSpot(BuildingType[] types)
        {
            this.uiManager.BuildingsDashboard.EnableDashboard();
            this.uiManager.BuildingsDashboard.DisableAllButtons();
            if (this.card.City != CityNames.None)
            {
                this.uiManager.BuildingsDashboard.EnableButtonsByType(types);
            }
            else
            {
                this.uiManager.BuildingsDashboard.EnableButtonsByType(this.card.Type);
            }

            this.uiManager.BuildingsDashboard.RestrictShipyardLevel1Button();
            this.uiManager.BuildingsDashboard.DistinctOnlyLowestLevelButtons();

            if (Container.Instance().GetGameState().GameRound == 2)
            {
                this.uiManager.BuildingsDashboard.Disable2ndRoundButtons();
            }
        }

        private void EnableDashboardForRebuild(BuildingType type, int level)
        {
            this.uiManager.BuildingsDashboard.EnableDashboard();
            this.uiManager.BuildingsDashboard.DisableAllButtons();
            this.uiManager.BuildingsDashboard.EnableButtonsByType(type, level);
            this.uiManager.BuildingsDashboard.RestrictShipyardLevel1Button();
            this.uiManager.BuildingsDashboard.DistinctOnlyLowestLevelButtons();

            if (Container.Instance().GetGameState().GameRound == 2)
            {
                this.uiManager.BuildingsDashboard.Disable2ndRoundButtons();
            }
        }

        private List<Building> FilterAvailableToRebuild(IEnumerable<Building> buildings)
        {
            List<Building> result = new List<Building>();
            foreach (var building in buildings)
            {
                var isResourceBuilding = building.BuildingType == BuildingType.SteelFactory || building.BuildingType == BuildingType.CoalMine;
                // var belongsToOtherPlayer = building.PlayerColor != Container.GetLocalPlayer().PlayerColor;
                //if (isResourceBuilding && belongsToOtherPlayer)
                //{
                //    if (building.BuildingType == BuildingType.SteelFactory && this.BoardAndMarketEmptyOfResources(BuildingType.SteelFactory))
                //    {
                //        result.Add(building);
                //    }

                //    if (building.BuildingType == BuildingType.CoalMine && this.BoardAndMarketEmptyOfResources(BuildingType.CoalMine))
                //    {
                //        result.Add(building);
                //    }
                //}
            }

            return result;
        }

        private List<Building> FilterAvailableToUpgrade(IEnumerable<Building> buildings)
        {
            return null;
            // return buildings.Where(building => building.PlayerColor == Container.GetLocalPlayer().PlayerColor && this.CanUpgradeBuilding(building)).ToList();
        }

        private List<Building> FilterAvailableToBuild(IEnumerable<Building> buildings)
        {
            var toFilter = buildings.ToArray();
            var result = new List<Building>();
            foreach (var building in toFilter)
            {
                var city = building.GetCity();

                //if (Container.i().GetGameState().GameRound == 1 && city.Buildings().Any(b => b.PlayerColor == Container.GetLocalPlayer().PlayerColor))
                //{
                //    continue;
                //}

                if (building.PlayerColor == PlayerColors.None)
                {
                    result.Add(building);
                }
            }

            return result;
        }

        private async void SetBuildingStates()
        {
            var toUpdateState = this.availableForRebuild.Union(this.availableForUpgrade);
            foreach (var building in toUpdateState)
            {
                await building.SetUpgradeStateAsync();
            }

            foreach (var building in this.availableForBuild)
            {
                building.SetState(new BuildingConstructState(building));
            }
        }

        private void SetBuildingVariables(IEnumerable<City> cities, BuildingType[] types)
        {
            cities = cities.ToArray();
            var buildingsToSetup = cities.SelectMany(c => c.Buildings().Where(b => b.AvailableBuildingType.Intersect(types).Any())).ToList();
            this.availableForRebuild = this.FilterAvailableToRebuild(buildingsToSetup.Where(b => types.Contains(b.BuildingType)));
            this.availableForUpgrade = this.FilterAvailableToUpgrade(buildingsToSetup);
            this.availableForBuild = this.FilterAvailableToBuild(buildingsToSetup);
        }

        private void DisableBuildTargets()
        {
            var buildings = this.mapGraph.GetCities().SelectMany(c => c.Buildings()).ToArray();
            foreach (var building in buildings)
            {
                building.SetIdleState();
            }
        }

        private void EnableResourcesToBuy(Building building)
        {
            if (building.price.CoalToBuildRemind > 0)
            {
                this.EnableCoal(building.GetCity());
            }

            if (building.price.SteelToBuildRemind > 0)
            {
                this.EnableSteel();
            }
        }

        private void EnableSteel()
        {
            var factories = this.mapGraph.GetBuildingsByType(BuildingType.SteelFactory).Where(sf => sf.SteelToSellRemind > 0).ToArray();
            if (factories.Any())
            {
                foreach (var factory in factories)
                {
                    factory.SetSellState();
                }
            }
            else
            {
                ClientEventAggregator.Publish(new EnableSteelMarketActionEvent());
            }
        }

        private void EnableCoal(City buildingCity)
        {
            var connectedCities = Pathfinder.SearchAvailableCities(buildingCity, MapGraph.instance);
            var buildingsWithCoal = connectedCities.SelectMany(c => c.Buildings()
                .Where(b => b.CoalToSellRemind > 0)).ToArray();

            if (buildingsWithCoal.Any())
            {
                foreach (var coalMine in buildingsWithCoal)
                {
                    coalMine.SetSellState();
                }
            }
            else if (Pathfinder.CanSellToWorldTrade(buildingCity, MapGraph.instance))
            {
                ClientEventAggregator.Publish(new EnableCoalMarketActionEvent());
            }
        }

        private bool BoardAndMarketEmptyOfResources(BuildingType type)
        {
            var allBuildings = this.mapGraph.GetBuildings();
            switch (type)
            {
                case BuildingType.SteelFactory when allBuildings.All(b => b.SteelToSellRemind == 0 && this.steelMarket.AvailableSteel == 0):
                case BuildingType.CoalMine when allBuildings.All(b => b.CoalToSellRemind == 0 && this.coalMarket.AvailableCoal == 0):
                    return true;
                default:
                    return false;
            }
        }

        private bool CanUpgradeBuilding(Building building)
        {
            var buttons = this.uiManager.BuildingsDashboard.columns.SelectMany(c => c.Buttons).ToList();
            var sameBuildingButton = buttons.Single(b => b.ButtonLevel == building.Level && b.BuildingType == building.BuildingType);
            if (sameBuildingButton.ButtonLevel != 4 && sameBuildingButton.DashboardButtonCounter.IsEmpty())
            {
                return true;
            }

            return false;
        }

        public bool CanFinishAction(ref string reason)
        {
            throw new System.NotImplementedException();
        }
    }
}
