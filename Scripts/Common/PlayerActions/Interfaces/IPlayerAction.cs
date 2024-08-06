using Assets.Scripts.Client.Events;

namespace Assets.Scripts.Common.PlayerActions.Interfaces
{
    public interface IPlayerAction : IRevertableAction, IEventHandler
    {
        void StartAction();

        void FinishAction();

        bool CanStartAction(ref string reason);

        bool CanFinishAction(ref string reason);
    }
}
