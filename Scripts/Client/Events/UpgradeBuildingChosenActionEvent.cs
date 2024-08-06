using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Client.Events
{
    class UpgradeBuildingChosenActionEvent : IActionEvent
    {
        public Building Building { get; set; }

        public UpgradeBuildingChosenActionEvent(Building building)
        {
            Building = building;
        }
    }
}
