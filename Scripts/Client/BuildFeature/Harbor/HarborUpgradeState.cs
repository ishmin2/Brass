using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.Harbor
{
    public class HarborUpgradeState : State<Building>
    {
        public HarborUpgradeState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, true, "Prefabs/Rebuild");
            OnClickAction = () => { ClientEventAggregator.Publish(new UpgradeBuildingChosenActionEvent(StateObject)); };
        }

        public override void OnStateExit()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
            OnClickAction = null;
        }
    }
}
