using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.ActionBoard.Events;
using Assets.Scripts.Client.GameFolder.ApprovePanel.Events;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Client.UI;
using Assets.Scripts.Common.PlayerActions.Interfaces;

namespace Assets.Scripts.Common.PlayerActions
{
    public class LoanAction : IEventHandler, IPlayerAction
    {
        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; } = new Dictionary<ISnapshotable, ISnapshot>();

        private readonly MoneyManager moneyManager;
        private readonly UIManager uiManager;
        private readonly MusicManager musicManager;
        private readonly PlayerPanel playerPanel;

        public LoanAction(MoneyManager manager, UIManager uiManager, MusicManager musicManager)
        {
            ClientEventAggregator.Subscribe(this);
            this.moneyManager = manager;
            this.uiManager = uiManager;
            this.musicManager = musicManager;
            this.playerPanel = Container.Instance().GetPlayerPanel();
            Snapshotables.Add(this.moneyManager, this.moneyManager.Save());
        }

        public bool CanStartAction(ref string reason)
        {
            bool hasError = false;
            if (this.playerPanel.Income <= -10)
            {
                reason = "Can't take more credit";
                hasError = true;
            }
            else if (!GameManager.i.GameState.LoadAvailable)
            {
                reason = "No more credit available this round";
                hasError = true;
            }

            if (hasError)
            {
                ClientEventAggregator.Publish(new CancelActionActionEvent());
            }

            return !hasError;
        }

        public void StartAction()
        {
            this.uiManager.EnableLoanOptions(this.playerPanel.Income);
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is LoanTakenActionEvent loanTakenEvent)
            {
                this.uiManager.DisableLoanOptions();
                this.moneyManager.GetLoan(loanTakenEvent.size);
            }
        }

        public void FinishAction()
        {
            this.musicManager.Play(SoundConstants.Yes);
            ClientEventAggregator.Unsubscribe(this);
            ClientEventAggregator.Publish(new EndActionEvent());
        }

        public void CancelAction()
        {
            foreach (var snapshotable in Snapshotables)
            {
                snapshotable.Key.Restore(snapshotable.Value);
            }

            this.uiManager.DisableLoanOptions();
            this.musicManager.Play(SoundConstants.No);
            ClientEventAggregator.Unsubscribe(this);
        }

        public bool CanFinishAction(ref string reason)
        {
            throw new System.NotImplementedException();
        }
    }
}
