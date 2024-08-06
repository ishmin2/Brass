namespace Assets.Scripts.Client.Events
{
    public class ValidationErrorActionEvent :IActionEvent
    {
        public string Message { get; }

        public ValidationErrorActionEvent(string message)
        {
            Message = message;
        }
    }
}
