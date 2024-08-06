using System;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class MarketsModel
    {
        public SteelMarketModel SteelMarket;

        public CoalMarketModel CoalMarket;

        public CottonMarketModel CottonMarket;
    }
}
