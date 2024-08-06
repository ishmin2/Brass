using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Constants;
using Assets.Scripts.MainScene.Data;
using UnityEngine;

namespace Assets.Scripts.MainScene.GameState
{
    public class DeckManager : MonoBehaviour
    {
        [HideInInspector]
        public static DeckManager i;

        public List<PlayerCard> Deck { get; private set; }

        void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else if (i == this)
            {
                Destroy(gameObject);
            }

            this.Deck = new List<PlayerCard>();
            this.LoadDeck();
        }

        public PlayerCard PopCard()
        {
            if (this.Deck.Count == 0)
            {
                return null;
            }

            var index = Random.Range(0, this.Deck.Count);
            var card = Deck[index];
            Deck.RemoveAt(index);
            return card;
        }

        public void LoadDeck()
        {
            // var playerCount = PlayersManager.i.PlayerCount;
            var playerCount = 2;
            this.Deck.AddRange(Resources.LoadAll<GameObject>($"Prefabs/DeckCards/{playerCount}Players").Select(x => x.GetComponent<PlayerCard>()));
        }

        public void LoadDeck(List<PlayerCard> deck)
        {
            this.Deck = deck;
        }
    }
}
