using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.WorldTrade
{
    public class WorldTradeSellState : State<Building>
    {
        public WorldTradeSellState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, true);
            OnClickAction = () =>
            {
                StateObject.SetState(new WorldTradeIdleState(StateObject));
                ClientEventAggregator.Publish(new WorldTradeChosenActionEvent());
            };
        }

        public override void OnStateExit()
        {
            OnClickAction = null;
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
        }
    }
}
