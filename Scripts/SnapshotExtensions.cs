using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.Roads;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Client.UI;
using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts
{
    public static class SnapshotExtensions
    {
        public static BuildingModel AsModel(this Building building)
        {
            return new BuildingModel
            {
                City = building.GetCity().Name,
                CitySlotNumber = building.CitySlotNumber,
                BuildingType = building.BuildingType,
                BuildingLevel = building.Level,
                PlayerColor = building.PlayerColor,
                CoalToSellRemind = building.CoalToSellRemind,
                SteelToSellRemind = building.SteelToSellRemind,
                isActivated = building.isActivated,
                CanWorldTrade = building.CanWorldTrade,
                VictoryPoints =  building.VictoryPoints,
                IncomeValue = building.IncomeValue
            };
        }

        public static RoadModel AsModel(this Road road)
        {
            return new RoadModel { CityA = road.CityA.Name, CityB = road.CityB.Name, PlayerColor = road.PlayerColor };
        }

        public static CoalMarketModel AsModel(this CoalMarket market)
        {
            return new CoalMarketModel { AvailableResources = market.AvailableCoal };
        }

        public static SteelMarketModel AsModel(this SteelMarket market)
        {
            return new SteelMarketModel { AvailableResources = market.AvailableSteel };
        }

        public static List<DeckCardModel> AsModel(this PlayerHand hand)
        {
            return new List<DeckCardModel>(hand.cards.Select(c => new DeckCardModel { BuildingType = c.Type, City = c.City }).ToList());
        }

        public static PlayerPanelModel AsModel(this PlayerPanel panel)
        {
            return new PlayerPanelModel { Money = panel.Money, Income = panel.Income, IncomeTrackPosition = panel.IncomeTrackPosition, CardCount = panel.CardCount, MoneySpent = panel.MoneySpent};
        }

        public static CottonMarketModel AsModel(this CottonMarketManager manager, List<CottonMarketCard> cards)
        {
            return new CottonMarketModel { TrackPosition = manager.currentCost, LastCard = manager.lastPlayedCard?.Cost, DeckCards = cards.Select(c => c.Cost).ToList() };
        }
    }
}
