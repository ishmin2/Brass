using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.DefaultBuilding
{
    public class BuildingConstructState : State<Building>
    {
        public BuildingConstructState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(this.StateObject.gameObject, true);
            OnClickAction = () =>
            {
                ClientEventAggregator.Publish(new BuildingToConstructChosenEvent(StateObject));
            };

        }

        public override void OnStateExit()
        {
            BuildingHelper.HighlightGameObject(this.StateObject.gameObject, false);
            OnClickAction = null;
        }
    }
}
