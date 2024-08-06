namespace Assets.Scripts.Client.Events
{
    public class CoalSellActionEvent : IActionEvent
    {
        public int sellCost { get; set; }

        public CoalSellActionEvent(int sellCost)
        {
            this.sellCost = sellCost;
        }
    }
}
