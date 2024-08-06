using System.Threading.Tasks;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Pathfind;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.SteelFactory
{
    public class SteelFactoryFinishState : State<Building>
    {
        public SteelFactoryFinishState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            // StateObject.PlayerColor = StateObject.PlayerColor == 0 ? Container.GetLocalPlayer().PlayerColor : StateObject.PlayerColor;

            var resource = Resources.Load<ResourceBox>("Prefabs/Factories/SteelBox");
            this.StateObject.ResourcesAmount = Object.Instantiate(resource, this.StateObject.transform);
            this.StateObject.ResourcesAmount.SetNumber(this.StateObject.SteelToSellRemind);

            if (StateObject.SteelToSellRemind > 0)
            {
                // Sell To Market if can.
                var city = BuildingExtension.GetCity(this.StateObject);
                var steelMarket = Container.GetSteelMarket();
                if (Pathfinder.CanSellToWorldTrade(city, MapGraph.instance))
                {
                    await this.SellResources(steelMarket);
                }
            }

            if (StateObject.SteelToSellRemind == 0)
            {
                if (this.StateObject.ResourcesAmount != null)
                {
                    Object.Destroy(this.StateObject.ResourcesAmount.gameObject);
                    this.StateObject.ResourcesAmount = null;
                }
                await BuildingHelper.ActivateBuilding(this.StateObject);
            }
        }

        private async Task SellResources(SteelMarket steelMarket)
        {
            while (steelMarket.AvailableSellCount() > 0)
            {
                if (StateObject.SteelToSellRemind > 0)
                {
                    StateObject.SteelToSellRemind--;

                    this.StateObject.ResourcesAmount.SetNumber(this.StateObject.SteelToSellRemind);
                    var resource = Resources.Load<GameObject>("Prefabs/Materials/Steel");
                    var instance = Object.Instantiate(resource);
                    instance.transform.position = StateObject.transform.position;

                    Vector3 to = Container.GetSteelMarket().transform.position;
                    Vector3 from = instance.transform.position;
                    while (Vector2.Distance(from, to) > 1)
                    {
                        instance.transform.position = Vector3.MoveTowards(from, to, 0.2f);
                        from = instance.transform.position;
                        await Task.Delay(20);
                    }

                    Object.Destroy(instance);

                    var sellBonus = steelMarket.Sell();
                    Container.GetMoneyManager().ChangeMoney(sellBonus);
                    // ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = Container.i().GetPlayerPanel().LocalMoney });
                }
                else
                {
                    break;
                }
            }
        }
    }
}
