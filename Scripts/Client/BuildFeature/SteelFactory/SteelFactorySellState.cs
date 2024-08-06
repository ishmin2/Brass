using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.SteelFactory
{
    public class SteelFactorySellState : State<Building>
    {
        public SteelFactorySellState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            if (this.StateObject.SteelToSellRemind == 0)
            {
                this.StateObject.SetState(new SteelFactoryIdleState(StateObject));
                return;
            }

            BuildingHelper.HighlightGameObject(StateObject.gameObject, true);
            OnClickAction = () =>
            {
                if (StateObject.SteelToSellRemind > 0)
                {
                    StateObject.SteelToSellRemind--;
                    StateObject.ResourcesAmount.SetNumber(StateObject.SteelToSellRemind);
                    ClientEventAggregator.Publish(new SteelSellActionEvent(0));
                }

                if (StateObject.SteelToSellRemind == 0)
                {
                    StateObject.SetState(new SteelFactoryIdleState(StateObject));
                }
            };
        }

        public override void OnStateExit()
        {
            OnClickAction = null;
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
        }
    }
}
