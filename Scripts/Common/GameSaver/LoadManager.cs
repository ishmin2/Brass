using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Server;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.GameSaver
{
    public class LoadManager : MonoBehaviour
    {
        void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(this.Load);
        }

        public void Load()
        {
            var save = SaveManager.save;
            //LoadGameState(save);
            //LoadBuildingsDashboard(save);
            //LoadDeck(save);
            this.LoadMarkets(save);
            //LoadPlayerState(save);
            this.LoadBoard(save);
        }

        public void Load(string saveFile)
        {
            var save = JsonUtility.FromJson<SaveFileModel>(saveFile);
            //LoadGameState(save);
            //LoadBuildingsDashboard(save);
            //LoadDeck(save);
            this.LoadMarkets(save);
            this.LoadPlayerState(save);
            this.LoadBoard(save);
        }

        private void LoadGameState(SaveFileModel save)
        {
            Container.Instance().GetGameState().CurrentPlayerRemindActions = save.GameState.RemindActionForPlayer;
            Container.Instance().GetGameState().GameRound = save.GameState.GameRound;
        }

        private void LoadBuildingsDashboard(SaveFileModel save)
        {
            var dbm = Container.GetDashboardManager();
            var buttons = dbm.columns.SelectMany(x => x.Buttons);

            foreach (var button in buttons)
            {
                var savedCopy = save.DashboardButtons.Single(
                    b => b.Level == button.ButtonLevel && b.BuildingType == button.BuildingType);

                button.DashboardButtonCounter.SetCount(savedCopy.RemindCount);
            }
        }

        private void LoadDeck(SaveFileModel save)
        {
            // TODO: create prefab folder without duplicates
            var cardPrefabs = Resources.LoadAll<PlayerCard>("Prefabs/DeckCards/2Players");

            List<PlayerCard> newDeck = new List<PlayerCard>();
            foreach (var savedCard in save.DeckCards)
            {
                newDeck.Add(cardPrefabs.First(cp => cp.City == savedCard.City && cp.Type == savedCard.BuildingType));
            }

            // deck.LoadDeck(newDeck);
        }

        private void LoadMarkets(SaveFileModel save)
        {
            Container.GetCoalMarket().Restore(new CoalMarketSnapshot(save.Markets.CoalMarket));
            Container.GetSteelMarket().Restore(new SteelMarketSnapshot(save.Markets.SteelMarket));

            var cottonMarket = Container.GetCottonMarket();
            cottonMarket.Reset();
            cottonMarket.SetTrackValue(save.Markets.CottonMarket.TrackPosition);
            var marketCards = Resources.LoadAll<CottonMarketCard>("Prefabs/CottonMarket");
            if (save.Markets.CottonMarket.LastCard.HasValue)
            {
                var lastPlayerCard = marketCards.SingleOrDefault(mc => mc.Cost == save.Markets.CottonMarket.LastCard);
                cottonMarket.SetDiscardCard(lastPlayerCard);
            }

            List<CottonMarketCard> cards = new List<CottonMarketCard>();
            foreach (var savedCard in save.Markets.CottonMarket.DeckCards)
            {
                cards.Add(marketCards.Single(mc => mc.Cost == savedCard));
            }

            ServerContainer.Instance().OuterMarketManager.Value.LoadDeck(cards);
        }

        private void LoadPlayerState(SaveFileModel save)
        {
            foreach (var player in save.Player)
            {
                var panel = Container.GetPlayerPanels().Single(p => p.Color == player.PlayerColor);
                panel.Money = player.PlayerPanelModel.Money;
                panel.Income = player.PlayerPanelModel.Income;
                panel.IncomeTrackPosition = player.PlayerPanelModel.IncomeTrackPosition;
                panel.CardCount = player.PlayerPanelModel.CardCount;
                panel.MoneySpent = player.PlayerPanelModel.MoneySpent;
            }

            var handSnapshot = new PlayerHandSnapshot(save.Player.First().PlayerHand);
            Container.GetPlayerHand().Restore(handSnapshot);
        }

        private void LoadBoard(SaveFileModel save)
        {
            var mapGraph = Container.GetMapGraph();
            var buildings = mapGraph.GetCities().SelectMany(c => c.Buildings()).ToArray();
            var roads = mapGraph.GetAllRoads();

            foreach (var building in buildings)
            {
                var snapshot = save.Board.Buildings.SingleOrDefault(b => b.City == building.GetCity().Name && b.CitySlotNumber == building.CitySlotNumber);
                if (snapshot != null)
                {
                    building.Restore(new BuildingSnapshot(snapshot));
                    continue;
                }

                building.Reset();
            }

            foreach (var road in roads)
            {
                var snapshot = save.Board.Roads.SingleOrDefault(b => b.CityA == road.CityA.Name && b.CityB == road.CityB.Name);
                if (snapshot != null)
                {
                    road.Restore(new RoadSnapshot(snapshot));
                    continue;
                }

                road.Reset();
            }
        }
    }
}
