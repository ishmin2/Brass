using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CottonMarket
{
    public class CottonMarketDeckBuilder
    {
        public List<CottonMarketCard> Deck { get; }

        private List<CottonMarketCard> prefabsCards;

        public CottonMarketDeckBuilder()
        {
            Deck = new List<CottonMarketCard>();
            this.prefabsCards = Resources.LoadAll<CottonMarketCard>("Prefabs/CottonMarket").ToList();
        }

        public void BuildPreset2PlayersDeck()
        {
            this.AddCardBatch(0, 1);
            this.AddCardBatch(1, 1);
            this.AddCardBatch(2, 3);
            this.AddCardBatch(3, 3);
            this.AddCardBatch(4, 1);
        }

        public void BuildPreset3PlayersDeck()
        {
            this.AddCardBatch(0, 1);
            this.AddCardBatch(1, 1);
            this.AddCardBatch(2, 4);
            this.AddCardBatch(3, 3);
            this.AddCardBatch(4, 1);
        }

        public void BuildPreset4PlayersDeck()
        {
            this.AddCardBatch(0, 2);
            this.AddCardBatch(1, 2);
            this.AddCardBatch(2, 4);
            this.AddCardBatch(3, 3);
            this.AddCardBatch(4, 1);
        }

        private void AddCardBatch(int cost, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Deck.Add(this.AddCard(cost));
            }
        }

        private CottonMarketCard AddCard(int cost)
        {
            switch (cost)
            {
                case 0:
                    return this.prefabsCards.Single(cp => cp.Cost == 0);
                case 1:
                    return this.prefabsCards.Single(cp => cp.Cost == -1);
                case 2:
                    return this.prefabsCards.Single(cp => cp.Cost == -2);
                case 3:
                    return this.prefabsCards.Single(cp => cp.Cost == -3);
                case 4:
                    return this.prefabsCards.Single(cp => cp.Cost == -4);
                default:
                    throw new ArgumentException();
            }
        }
    }
}

