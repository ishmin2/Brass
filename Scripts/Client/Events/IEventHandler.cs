namespace Assets.Scripts.Client.Events
{
    public interface IEventHandler
    {
        void HandleEvent<T>(T eventType) where T : IActionEvent;
    }
}
