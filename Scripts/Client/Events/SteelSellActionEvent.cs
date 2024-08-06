namespace Assets.Scripts.Client.Events
{
    public class SteelSellActionEvent : IActionEvent
    {
        public int sellCost { get; set; }

        public SteelSellActionEvent(int sellCost)
        {
            this.sellCost = sellCost;
        }
    }
}
