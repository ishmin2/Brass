using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.CottonFactory
{
    public class CottonFactoryUpgradeState : State<Building>
    {
        public CottonFactoryUpgradeState(Building stateObject) : base(stateObject)
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
