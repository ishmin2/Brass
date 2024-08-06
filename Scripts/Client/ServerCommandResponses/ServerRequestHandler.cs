using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.CottonFactory;
using Assets.Scripts.Client.BuildFeature.Harbor;
using Assets.Scripts.Client.BuildFeature.Roads.States;
using Assets.Scripts.Client.BuildFeature.Shipyard;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Common;
using Assets.Scripts.Server;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Client.ServerCommandResponses
{
    public class ServerRequestHandler
    {
        public async Task ResponseAsync(MarketBuyResourceRequestModel marketBuyResourceRequestModel)
        {
            switch (marketBuyResourceRequestModel.MarketType)
            {
                case MarketType.SteelMarket:
                    Container.GetSteelMarket().Buy();
                    break;
                case MarketType.CoalMarket:
                    Container.GetCoalMarket().Buy();
                    break;
            }
        }

        public async Task ResponseAsync(RoadBuiltRequestModel roadBuiltRequestModel)
        {
            var road = Container.GetMapGraph().GetAllRoads()
                .Single(r => (r.CityA.Name == roadBuiltRequestModel.CityA || r.CityA.Name == roadBuiltRequestModel.CityB) &&
                             (r.CityB.Name == roadBuiltRequestModel.CityA || r.CityB.Name == roadBuiltRequestModel.CityB));
            road.SetState(new RoadBuiltState(road, (PlayerColors)roadBuiltRequestModel.ownerID));
        }

        public async Task ResponseAsync(BuildingSellResourceRequestModel buildingSellResource)
        {
            var city = Container.GetMapGraph().GetCities().Single(c => c.Name == buildingSellResource.City);
            var building = city.Buildings().Single(b => b.CitySlotNumber == buildingSellResource.BuildingSlot);
            await building.CoalMine.RpcSell(building.currentState);
        }

        public async Task ResponseAsync(BuildingConstructedRequestModel buildingConstructed)
        {
            var city = Container.GetMapGraph().GetCities().Single(c => c.Name == buildingConstructed.City);
            var buildingSlot = city.Buildings().Single(b => b.CitySlotNumber == buildingConstructed.BuildingSlot);

            buildingSlot.PlayerColor = buildingConstructed.PlayerColor;
            buildingSlot.BuildingType = buildingConstructed.BuildingType;
            buildingSlot.CoalToSellRemind = buildingConstructed.CoalToSellRemind;
            buildingSlot.SteelToSellRemind = buildingConstructed.SteelToSellRemind;
            buildingSlot.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"AllSprites/{buildingConstructed.PlayerColor}/{buildingConstructed.BuildingType}_def_{buildingConstructed.BuildingLevel}");
            await buildingSlot.SetFinishStateAsync();
        }

        public async Task ResponseAsync(ChangeMoneyRequestModel changeMoney)
        {
            //var panel = ServerContainer.i().ServerPlayer.ServerData.PlayerPanels.Single(pp => pp.Color == changeMoney.PlayerColor);
            //panel.Money += changeMoney.Money;
            //panel.Income += changeMoney.Income;
            //panel.IncomeTrackPosition += changeMoney.IncomeTrackPosition;
            //if (!changeMoney.IsLoan)
            //{
            //    panel.MoneySpent += changeMoney.Money;
            //}
        }

        public async Task ResponseAsync(CardPlayedRequestModel cardPlayed)
        {
            var card = Resources.LoadAll<PlayerCard>("Prefabs/DeckCards/2Players")
                .First(c => c.Type == cardPlayed.BuildingType && c.City == cardPlayed.City);

            var instance = Object.Instantiate(card.gameObject, Container.GetCanvas().transform);
            var rect = (RectTransform)Container.GetCanvas().transform;
            instance.transform.position = new Vector3(rect.rect.width / 2, rect.rect.height / 2);
            instance.transform.localScale = new Vector3(3, 3);
            Object.Destroy(instance.GetComponent<PlayerCard>());
            Object.Destroy(instance.gameObject, 2);
            await Task.Delay(3000);
        }

        public async Task ResponseAsync(ActivateBuildingRequestModel buildingModel)
        {
            var building = Container.GetMapGraph().GetBuildings().Single(b =>
                b.GetCity().Name == buildingModel.City && b.CitySlotNumber == buildingModel.CitySlotNumber);

            switch (building.BuildingType)
            {
                case BuildingType.CottonFactory:
                    await building.SetStateAsync(new CottonFactoryActivatedState(building));
                    break;
                case BuildingType.Harbor:
                    await building.SetStateAsync(new HarborActivatedState(building));
                    break;
                case BuildingType.Shipyard:
                    await building.SetStateAsync(new ShipyardFinishState(building));
                    break;
            }
        }
    }
}
