using System.Collections.Generic;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.ActionBoard.Events;
using Assets.Scripts.Common.PlayerActions.Interfaces;

namespace Assets.Scripts.Common.PlayerActions
{
    public class PassTurnAction : IPlayerAction
    {
        private readonly MusicManager musicManager;
        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; } = new Dictionary<ISnapshotable, ISnapshot>();

        public PassTurnAction(MusicManager musicManager)
        {
            this.musicManager = musicManager;
        }

        public void CancelAction()
        {
            foreach (var snapshotable in Snapshotables)
            {
                snapshotable.Key.Restore(snapshotable.Value);
            }
            this.musicManager.Play(SoundConstants.No);
        }

        public void StartAction()
        {
        }

        public void FinishAction()
        {
            this.musicManager.Play(SoundConstants.Yes);
            ClientEventAggregator.Publish(new EndActionEvent());
        }

        public bool CanStartAction(ref string reason)
        {
            return true;
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            throw new System.NotImplementedException();
        }

        public bool CanFinishAction(ref string reason)
        {
            throw new System.NotImplementedException();
        }
    }
}
