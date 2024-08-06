using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Client.Events
{
    public class BuildingToConstructChosenEvent : IActionEvent
    {
        public Building Building { get; }

        public BuildingToConstructChosenEvent(Building building)
        {
            Building = building;
        }
    }
}
