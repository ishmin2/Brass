using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts.Client.Interactions.SteelMarket
{
    public class SteelMarketSnapshot : ISnapshot
    {
        public SteelMarketModel Model { get; }

        public SteelMarketSnapshot(SteelMarketModel steelMarketModel)
        {
            Model = steelMarketModel;
        }
    }
}
