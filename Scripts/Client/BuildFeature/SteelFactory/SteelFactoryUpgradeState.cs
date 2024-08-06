using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.SteelFactory
{
    public class SteelFactoryUpgradeState : State<Building>
    {
        public SteelFactoryUpgradeState(Building stateObject) : base(stateObject)
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
