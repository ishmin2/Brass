using Assets.Scripts.Client.BuildFeature.Roads;

namespace Assets.Scripts.Client.Events
{
    public class RoadChosenActionEvent : IActionEvent
    {
        public Road road { get; set; }

        public RoadChosenActionEvent(Road road)
        {
            this.road = road;
        }
    }
}
