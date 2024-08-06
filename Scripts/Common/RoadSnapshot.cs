using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts.Common
{
    public class RoadSnapshot : ISnapshot
    {
        public RoadModel Model { get; }

        public RoadSnapshot(RoadModel model)
        {
            this.Model = model;
        }
    }
}
