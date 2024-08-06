using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Client.Events
{
    public class CottonFactoryChosenActionEvent : IActionEvent
    {
        public Building Building { get; set; }

        public CottonFactoryChosenActionEvent(Building building)
        {
            Building = building;
        }
    }
}
