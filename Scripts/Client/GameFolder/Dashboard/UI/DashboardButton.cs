using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.GameFolder.Dashboard.States;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.GameFolder.Dashboard.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Button))]
    public class DashboardButton : MonoBehaviour, IDashboardButton
    {
        public int ButtonLevel;
        public DashboardButtonCounter DashboardButtonCounter;
        public BuildingType BuildingType;
        public Building FactoryPrefab;

        public Image Image { get; private set; }
        public Button Button { get; private set; }

        void Awake()
        {
            this.DashboardButtonCounter = this.GetComponentInChildren<DashboardButtonCounter>();
            this.BuildingType = transform.parent.GetComponent<DashboardColumn>().Type;
            Image = this.GetComponent<Image>();
            Button = this.GetComponent<Button>();
        }

        public NoUpdateState<DashboardButton> currentState { get; set; }
        
        public void SetActiveState()
        {
            this.SetState(new DashboardButtonActiveState(this));
        }

        public void SetLockState()
        {
            this.SetState(new DashboardButtonLockState(this));
        }

        public void SetRestrictState()
        {
            this.SetState(new DashboardButtonRestrictState(this));
        }

        public void SetIdleState()
        {
            this.SetState(new DashboardButtonIdleState(this));
        }

        private void SetState(NoUpdateState<DashboardButton> state)
        {
            currentState?.OnStateExit();
            currentState = state;
            currentState?.OnStateEnter();
        }
    }
}
