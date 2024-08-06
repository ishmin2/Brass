using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CottonMarket
{
    public class CottonMarketCard : MonoBehaviour
    {
        public int Cost;

        public CottonMarketCard(int cost)
        {
            this.Cost = cost;
        }
    }
}
