using System.Collections.Generic;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.GameFolder.Dashboard
{
    public class DashboardManagerSnapshot : ISnapshot
    {
        public List<SnapshotTuple> Buttons { get; }

        public DashboardManagerSnapshot(List<SnapshotTuple> buttons)
        {
            Buttons = new List<SnapshotTuple>(buttons);
        }
    }

    public class SnapshotTuple
    {
        public BuildingType Type;

        public int Level;

        public int Count;
    }
}
