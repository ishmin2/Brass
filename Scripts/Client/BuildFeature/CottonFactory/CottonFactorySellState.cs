using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.CottonFactory
{
    public class CottonFactorySellState : State<Building>
    {
        public CottonFactorySellState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, true);
            OnClickAction = () =>
            {
                StateObject.SetState(new CottonFactoryIdleState(StateObject));
                ClientEventAggregator.Publish(new CottonFactoryChosenActionEvent(StateObject));
            };
        }

        public override void OnStateExit()
        {
            OnClickAction = null;
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
        }
    }
}
