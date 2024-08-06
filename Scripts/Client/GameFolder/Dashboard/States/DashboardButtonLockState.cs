using Assets.Scripts.Client.GameFolder.Dashboard.UI;
using UnityEngine;

namespace Assets.Scripts.Client.GameFolder.Dashboard.States
{
    public class DashboardButtonLockState : NoUpdateState<DashboardButton>
    {
        public DashboardButtonLockState(DashboardButton stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            this.StateObject.Button.enabled = false;
            this.StateObject.Image.sprite = Resources.Load<Sprite>("Locked");
        }

        public override void OnStateExit()
        {
            this.StateObject.Button.enabled = true;
        }
    }
}
