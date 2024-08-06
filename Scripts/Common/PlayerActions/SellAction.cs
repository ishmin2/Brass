//using System.Collections.Generic;
//using System.Linq;
//using Assets.Scripts.Client;
//using Assets.Scripts.Client.BuildFeature;
//using Assets.Scripts.Client.BuildFeature.CottonFactory;
//using Assets.Scripts.Client.Commands.Models;
//using Assets.Scripts.Client.Events;
//using Assets.Scripts.Client.GameFolder.ActionBoard.Events;
//using Assets.Scripts.Client.GameFolder.ApprovePanel.Events;
//using Assets.Scripts.Client.Interactions.CottonMarket;
//using Assets.Scripts.Client.MoneyController;
//using Assets.Scripts.Common.Pathfind;
//using Assets.Scripts.Common.PlayerActions.Interfaces;
//using UnityEngine;

//namespace Assets.Scripts.Common.PlayerActions
//{
//    public class SellAction : MonoBehaviour, IPlayerAction
//    {
//        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; } = new Dictionary<ISnapshotable, ISnapshot>();

//        private readonly BuildingManager buildingManager;
//        private readonly CottonMarketManager cottonMarketManager;
//        private readonly MoneyManager moneyManager;
//        private readonly MusicManager musicManager;
//        private Building currentCotton;
//        private bool canFinishAction;
//        private bool canCancelAction = true;


//        public SellAction(BuildingManager buildingManager, CottonMarketManager cottonMarketManager,
//            MoneyManager moneyManager, MusicManager musicManager)
//        {
//            this.buildingManager = buildingManager;
//            this.cottonMarketManager = cottonMarketManager;
//            this.moneyManager = moneyManager;
//            this.musicManager = musicManager;

//            Snapshotables.Add(this.moneyManager, this.moneyManager.Save());

//            var harbors = MapGraph.i.GetBuildingsByType(BuildingType.Harbor).Where(x => x.isActivated == false);
//            var cottons = MapGraph.i.GetBuildingsByType(BuildingType.CottonFactory).Where(x => x.isActivated == false);
//            foreach (var building in harbors.Union(cottons).ToArray())
//            {
//                Snapshotables.Add(building, building.Save());
//            }
//            ClientEventAggregator.Subscribe(this);
//        }

//        public bool CanStartAction(ref string reason)
//        {
//            bool hasError = false;

//            var cottonsPerPlayer = MapGraph.i.GetBuildingsByType(BuildingType.CottonFactory).Where(x =>
//                x.isActivated == false && x.PlayerColor == Container.GetLocalPlayer().PlayerColor).ToList();
//            if (!cottonsPerPlayer.Any())
//            {
//                reason = "No available cotton factories to sell";
//                hasError = true;
//            }else if (!this.GetFactoroiesWithSellAvailability().Any())
//            {
//                reason = "No available trade routes to sell";
//                ClientEventAggregator.Publish(new CancelActionActionEvent());
//                hasError = true;
//            }

//            return !hasError;
//        }

//        public void StartAction()
//        {
//            this.buildingManager.ActivateBuildings(this.GetFactoroiesWithSellAvailability());
//        }

//        public void FinishAction()
//        {
//            if (this.canFinishAction)
//            {
//                this.musicManager.Play(SoundConstants.Yes);

//                ClientEventAggregator.Unsubscribe(this);
//                ClientEventAggregator.Publish(new EndActionEvent());
//            }
//            else
//            {
//                this.musicManager.Play(SoundConstants.No);
//            }
//        }

//        public void CancelAction()
//        {
//            if (!this.canCancelAction)
//            {
//                ClientEventAggregator.Publish(new ValidationErrorActionEvent("Can't cancel sell Action after world trade"));
//                throw new CancelSellActionException("");
//            }
//        }

//        public async void HandleEvent<T>(T eventType) where T : IActionEvent
//        {
//            if (eventType is CottonFactoryChosenActionEvent cottonFactoryChosenEvent)
//            {
//                this.canFinishAction = false;
//                this.currentCotton = cottonFactoryChosenEvent.Building;
//                this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.CottonFactory);

//                var city = cottonFactoryChosenEvent.Building.GetCity();
//                var connectedCities = Pathfinder.SearchAvailableCities(city, MapGraph.i);
//                Building[] buildingsToActivate;
//                if (this.cottonMarketManager.CanTrade())
//                {
//                    buildingsToActivate = connectedCities.SelectMany(c => c.Buildings().Where(b =>
//                        b.BuildingType == BuildingType.Harbor ||
//                        b.BuildingType == BuildingType.WorldTrade ||
//                        b.CanWorldTrade)).ToArray();
//                }
//                else
//                {
//                    buildingsToActivate = connectedCities.SelectMany(c => c.Buildings().Where(b =>
//                        b.BuildingType == BuildingType.Harbor)).ToArray();
//                }

//                this.buildingManager.ActivateBuildings(buildingsToActivate);
//            }

//            if (eventType is HarborChosenActionEvent harborChosenEvent)
//            {
//                this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.Harbor);
//                this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.WorldTrade);

//                if (!harborChosenEvent.Building.isActivated)
//                {
//                    await this.currentCotton.SetStateAsync(new CottonFactoryActivatedState(this.currentCotton));
//                    await harborChosenEvent.Building.SetStateAsync(new CottonFactoryActivatedState(harborChosenEvent.Building));

//                    ClientEventAggregator.Publish(new ActivateBuildingRequestModel { City = this.currentCotton.GetCity().Name, CitySlotNumber = this.currentCotton.CitySlotNumber });
//                    ClientEventAggregator.Publish(new ActivateBuildingRequestModel { City = harborChosenEvent.Building.GetCity().Name, CitySlotNumber = harborChosenEvent.Building.CitySlotNumber });

//                    this.canFinishAction = true;
//                    this.TryNextSell();
//                }
//                else
//                {
//                    ClientEventAggregator.Publish(new WorldTradeChosenActionEvent());
//                }
//            }

//            if (eventType is WorldTradeChosenActionEvent)
//            {
//                this.canCancelAction = false;
//                this.canFinishAction = false;

//                this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.Harbor);
//                this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.WorldTrade);

//                Container.GetLocalPlayer().CmdDrawOuterMarketCard();
//            }

//            if (eventType is CottonCardDrawnEvent)
//            {
//                if (this.cottonMarketManager.CanTrade())
//                {
//                    // TODO: rpc draw cotton card
//                    this.moneyManager.ChangeIncome(this.cottonMarketManager.GetSellBonus());
//                    await this.currentCotton.SetStateAsync(new CottonFactoryActivatedState(this.currentCotton));
//                    ClientEventAggregator.Publish(new ActivateBuildingRequestModel { City = this.currentCotton.GetCity().Name, CitySlotNumber = this.currentCotton.CitySlotNumber });
//                }

//                this.canFinishAction = true;
//                this.TryNextSell();
//            }
//        }

//        private void TryNextSell()
//        {
//            if (this.GetFactoroiesWithSellAvailability().Any())
//            {
//                this.StartAction();
//            }
//        }

//        private List<Building> GetFactoroiesWithSellAvailability()
//        {
//            var cottonsPerPlayer = MapGraph.i.GetBuildingsByType(BuildingType.CottonFactory).Where(x => x.isActivated == false && x.PlayerColor == Container.GetLocalPlayer().PlayerColor).ToList();
//            var result = new List<Building>();

//            foreach (var cottonBuilding in cottonsPerPlayer)
//            {
//                var connectedCities = Pathfinder.SearchAvailableCities(cottonBuilding.GetCity(), MapGraph.i);
//                var harbors = connectedCities.SelectMany(c => c.Buildings().Where(b => b.BuildingType == BuildingType.Harbor));
//                if (!harbors.All(h => h.isActivated))
//                {
//                    result.Add(cottonBuilding);
//                }else if (connectedCities.Any(bs => bs.Buildings().Any(b => b.CanWorldTrade)) && this.cottonMarketManager.CanTrade())
//                {
//                    result.Add(cottonBuilding);
//                }
//            }

//            return result;
//        }

//        public bool CanFinishAction(ref string reason)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
