using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.Harbor
{
    public class HarborSellState : State<Building>
    {
        public HarborSellState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, true);
            OnClickAction = () =>
            {
                StateObject.SetState(new HarborIdleState(StateObject));
                ClientEventAggregator.Publish(new HarborChosenActionEvent(StateObject));
            };
        }

        public override void OnStateExit()
        {
            OnClickAction = null;
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
        }
    }
}
