namespace Assets.Scripts.Client.Events
{
    public class DropCardActionEvent : IActionEvent
    {
        public PlayerCard Card { get; }

        public DropCardActionEvent(PlayerCard card)
        {
            Card = card;
        }
    }
}
