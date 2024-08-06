using Assets.Scripts.Client.GameFolder.Dashboard.Events;
using Assets.Scripts.Client.GameFolder.Dashboard.UI;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.GameFolder.Dashboard.States
{
    public class DashboardButtonActiveState : NoUpdateState<DashboardButton>
    {
        public DashboardButtonActiveState(DashboardButton stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            if(this.StateObject.DashboardButtonCounter.IsEmpty())
            {
                this.StateObject.SetIdleState();
                return;
            }

            this.StateObject.Button.enabled = true;
            this.StateObject.Image.sprite = Resources.Load<Sprite>("SelectionBox-72x72");
            this.StateObject.GetComponent<Button>().onClick.AddListener(this.OnClick);
        }

        public override void OnStateExit()
        {
            this.StateObject.GetComponent<Button>().onClick.RemoveListener(this.OnClick);
        }

        void OnClick()
        {
            this.StateObject.DashboardButtonCounter.RemoveOne();
            ClientEventAggregator.Publish(new DashboardBuildingChosenActionEvent(this.StateObject.FactoryPrefab));
            if (this.StateObject.DashboardButtonCounter.IsEmpty())
            {
                this.StateObject.SetIdleState();
            }
        }
    }
}
