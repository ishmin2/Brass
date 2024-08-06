using System.Collections.Generic;
using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Common.PlayerActions.Interfaces
{
    public interface IRevertableAction
    {
        Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; }

        void CancelAction();
    }
}
