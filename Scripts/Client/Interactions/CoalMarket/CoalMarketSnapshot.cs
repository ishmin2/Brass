using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts.Client.Interactions.CoalMarket
{
    public class CoalMarketSnapshot : ISnapshot
    {
        public CoalMarketModel Model { get; }

        public CoalMarketSnapshot(CoalMarketModel coalMarket)
        {
            Model = coalMarket;
        }
    }
}
