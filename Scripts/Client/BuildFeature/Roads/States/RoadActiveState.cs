using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.Roads.States
{
    public class RoadActiveState : State<Road>
    {
        public RoadActiveState(Road stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(this.StateObject.gameObject, true);
            OnClickAction = () =>
            {
                ClientEventAggregator.Publish(new RoadChosenActionEvent(this.StateObject));
            };
        }

        public override void OnStateExit()
        {
            BuildingHelper.HighlightGameObject(this.StateObject.gameObject, false);
            OnClickAction = null;
        }
    }
}
