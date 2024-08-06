using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Events;

namespace Assets.Scripts.Client.GameFolder.Dashboard.Events
{
    public class DashboardBuildingChosenActionEvent : IActionEvent
    {
        public Building Building { get; set; }

        public DashboardBuildingChosenActionEvent(Building building)
        {
            Building = building;
        }
    }
}
