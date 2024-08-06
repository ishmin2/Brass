using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.ActionBoard.Events;
using Assets.Scripts.Client.GameFolder.ApprovePanel.Events;
using Assets.Scripts.Client.GameFolder.Dashboard.Events;
using Assets.Scripts.Client.Interactions.CottonMarket.Events;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Common.Pathfind;
using Assets.Scripts.Common.PlayerActions.Interfaces;

namespace Assets.Scripts.Common.PlayerActions
{
    public class DevelopAction : IPlayerAction
    {
        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; } = new Dictionary<ISnapshotable, ISnapshot>();

        private readonly BuildingManager buildingManager;
        private readonly UIManager uiManager;
        private readonly MapGraph mapGraph;
        private readonly MoneyManager moneyManager;
        private readonly MusicManager musicManager;
        private int developCount;
        private bool canFinishAction;

        public DevelopAction(BuildingManager buildingManager, UIManager uiManager, MapGraph mapGraph,
            MoneyManager moneyManager, MusicManager musicManager)
        {
            this.buildingManager = buildingManager;
            this.uiManager = uiManager;
            this.mapGraph = mapGraph;
            this.moneyManager = moneyManager;
            this.musicManager = musicManager;

            this.Snapshotables.Add(moneyManager, moneyManager.Save());
            this.Snapshotables.Add(uiManager.BuildingsDashboard, uiManager.BuildingsDashboard.Save());
            foreach (var building in this.mapGraph.GetBuildingsByType(BuildingType.SteelFactory))
            {
                this.Snapshotables.Add(building, building.Save());
            }
            ClientEventAggregator.Subscribe(this);
        }

        public bool CanStartAction(ref string reason)
        {
            bool hasError = false;
            var availableSteelMines = this.mapGraph.GetBuildingsByType(BuildingType.SteelFactory).Any(x => x.SteelToSellRemind > 0);
            var canBuyFromMarket =
                Container.Instance().GetPlayerPanel().Money >=
                Container.GetSteelMarket().CurrentCost;
            if (this.uiManager.BuildingsDashboard.columns.SelectMany(c => c.Buttons).All(b => b.DashboardButtonCounter.IsEmpty()))
            {
                reason = "No upgrades available";
                hasError = true;
            }
            else if (!availableSteelMines && !canBuyFromMarket)
            {
                reason = "No money to buy upgrade";
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
            this.ActivateSteelSellPoints();
        }

        public void FinishAction()
        {
            var enoughMoney = Container.Instance().GetPlayerPanel().LocalMoney > 0;
            var upgradeChosen = this.canFinishAction;
            if (upgradeChosen && enoughMoney)
            {
                this.DeactivateSteelPoints();
                this.musicManager.Play(SoundConstants.Yes);
                ClientEventAggregator.Unsubscribe(this);
                ClientEventAggregator.Publish(new EndActionEvent());
            }
            else
            {
                ClientEventAggregator.Publish(!upgradeChosen
                    ? new ValidationErrorActionEvent("Choose upgrade before finish turn")
                    : new ValidationErrorActionEvent("Not enough money"));
            }
        }

        public void CancelAction()
        {
            ClientEventAggregator.Unsubscribe(this);
            ClientEventAggregator.Publish(new CancelActionActionEvent());
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is MarketBuyResourceRequestModel steelSellEvent && steelSellEvent.MarketType == MarketType.SteelMarket)
            {
                this.uiManager.EnableBuildingDashboard();
                this.uiManager.BuildingsDashboard.ActivateAllButtons().DistinctOnlyLowestLevelButtons();
                this.DeactivateSteelPoints();
                this.moneyManager.ChangeMoney(-steelSellEvent.Cost);
                // ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = Container.i().GetPlayerPanel().LocalMoney });
                this.canFinishAction = false;
            }

            if (eventType is SteelSellActionEvent)
            {
                this.uiManager.EnableBuildingDashboard();
                this.uiManager.BuildingsDashboard.ActivateAllButtons().DistinctOnlyLowestLevelButtons();
                this.DeactivateSteelPoints();
                this.canFinishAction = false;
            }

            if (eventType is DashboardBuildingChosenActionEvent)
            {
                this.uiManager.DisableBuildingDashboard();
                this.developCount++;
                this.canFinishAction = true;

                if (this.developCount == 1)
                {
                    this.ActivateSteelSellPoints();
                }
            }
        }

        private void ActivateSteelSellPoints()
        {
            var steelFactories = this.buildingManager.ActivateBuildingByFactoryType(BuildingType.SteelFactory).Where<Building>(b => b.SteelToSellRemind > 0).ToList();
            if (!steelFactories.Any())
            {
                ClientEventAggregator.Publish(new EnableSteelMarketActionEvent());
            }
        }

        private void DeactivateSteelPoints()
        {
            this.buildingManager.DeactivateBuildingByFactoryType(BuildingType.SteelFactory);
            ClientEventAggregator.Publish(new DisableSteelMarketActionEvent());
        }

        public bool CanFinishAction(ref string reason)
        {
            throw new System.NotImplementedException();
        }
    }
}