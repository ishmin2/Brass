namespace Assets.Scripts.Client.GameFolder.Dashboard.UI
{
    public interface IDashboardButton
    {
        NoUpdateState<DashboardButton> currentState { get; set; }

        void SetActiveState();

        void SetLockState();

        void SetRestrictState();

        void SetIdleState();
    }
}
