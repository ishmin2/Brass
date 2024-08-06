using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts.Client.BuildFeature
{
    public class BuildingSnapshot : ISnapshot
    {
        public BuildingModel Model { get; }

        public BuildingSnapshot(BuildingModel buildingModel)
        {
            this.Model = buildingModel;
        }
    }
}
