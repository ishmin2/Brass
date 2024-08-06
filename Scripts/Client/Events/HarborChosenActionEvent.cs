using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Client.Events
{
    public class HarborChosenActionEvent : IActionEvent
    {
        public Building Building { get; set; }

        public HarborChosenActionEvent(Building building)
        {
            Building = building;
        }
    }
}
