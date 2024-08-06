using Assets.Scripts.Client.Interactions.CottonMarket;

namespace Assets.Scripts.Client.Events
{
    public class DrawOuterMarketCardEvent : IActionEvent
    {
        public CottonMarketCard CottonMarketCard;

        public DrawOuterMarketCardEvent(CottonMarketCard card)
        {
            this.CottonMarketCard = card;
        }
    }
}
