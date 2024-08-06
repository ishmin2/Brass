namespace Assets.Scripts.Client.Events
{
    public class DrawCardActionEvent : IActionEvent
    {
        public PlayerCard PlayerCard;

        public DrawCardActionEvent(PlayerCard card)
        {
            this.PlayerCard = card;
        }
    }
}
