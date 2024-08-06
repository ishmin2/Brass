using Assets.Scripts.Client.GameFolder.Dashboard.UI;

namespace Assets.Scripts.Client.GameFolder.Dashboard.States
{
    public class DashboardButtonIdleState : NoUpdateState<DashboardButton>
    {
        public DashboardButtonIdleState(DashboardButton stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            this.StateObject.Button.enabled = false;
            this.StateObject.Image.enabled = false;
        }

        public override void OnStateExit()
        {
            this.StateObject.Button.enabled = true;
            this.StateObject.Image.enabled = true;
        }
    }
}
