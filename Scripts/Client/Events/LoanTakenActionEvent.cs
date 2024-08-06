using Assets.Scripts.Client.MoneyController;

namespace Assets.Scripts.Client.Events
{
    public class LoanTakenActionEvent : IActionEvent
    {
        public LoanSize size { get; }

        public LoanTakenActionEvent(LoanSize size)
        {
            this.size = size;
        }
    }
}
