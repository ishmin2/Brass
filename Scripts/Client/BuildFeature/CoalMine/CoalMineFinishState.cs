using System.Threading.Tasks;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Pathfind;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.CoalMine
{
    public class CoalMineFinishState : State<Building>
    {
        public CoalMineFinishState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            // StateObject.PlayerColor = StateObject.PlayerColor == 0 ? Container.GetLocalPlayer().PlayerColor : StateObject.PlayerColor;

            var resource = Resources.Load<ResourceBox>("Prefabs/Factories/CoalBox");
            this.StateObject.ResourcesAmount = Object.Instantiate(resource, this.StateObject.transform);
            this.StateObject.ResourcesAmount.SetNumber(this.StateObject.CoalToSellRemind);

            var city = BuildingExtension.GetCity(this.StateObject);
            if (StateObject.CoalToSellRemind > 0 && Pathfinder.CanSellToWorldTrade(city, MapGraph.instance))
            {
                var coalMarket = Container.GetCoalMarket();
                await this.SellResources(coalMarket);
            }

            if (StateObject.CoalToSellRemind == 0)
            {
                if (this.StateObject.ResourcesAmount != null)
                {
                    Object.Destroy(this.StateObject.ResourcesAmount.gameObject);
                    this.StateObject.ResourcesAmount = null;
                }
                await BuildingHelper.ActivateBuilding(this.StateObject);
            }
        }

        private async Task SellResources(CoalMarket coalMarket)
        {
            while (coalMarket.AvailableSellCount() > 0)
            {
                if (StateObject.CoalToSellRemind > 0)
                {
                    StateObject.CoalToSellRemind--;

                    this.StateObject.ResourcesAmount.SetNumber(this.StateObject.CoalToSellRemind);
                    var resource = Resources.Load<GameObject>("Prefabs/Materials/Coal");
                    var instance = Object.Instantiate(resource);
                    instance.transform.position = StateObject.transform.position;

                    Vector3 to = Container.GetCoalMarket().transform.position;
                    Vector3 from = instance.transform.position;
                    while (Vector2.Distance(from, to) > 1)
                    {
                        instance.transform.position = Vector3.MoveTowards(from, to, 0.2f);
                        from = instance.transform.position;
                        await Task.Delay(20);
                    }

                    Object.Destroy(instance);
                   
                    var sellBonus = coalMarket.Sell();
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
