using System.Collections.Generic;
using Assets.Scripts.Client.Interactions.CottonMarket;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Server
{
    public class OuterMarketManager
    {
        public List<CottonMarketCard> Deck { get; private set; } = new List<CottonMarketCard>();

        public OuterMarketManager()
        {
            this.LoadDeck();
        }

        public CottonMarketCard PopCard()
        {
            if (this.Deck.Count == 0)
            {
                return null;
            }

            var index = Random.Range(0, Deck.Count);
            var card = Deck[index];
            Deck.RemoveAt(index);
            return card;
        }

        public void LoadDeck()
        {
            var deckBuilder = new CottonMarketDeckBuilder();
            deckBuilder.BuildPreset2PlayersDeck();
            Deck = deckBuilder.Deck;
        }

        public void LoadDeck(List<CottonMarketCard> deck)
        {
            Deck = deck;
        }
    }
}
