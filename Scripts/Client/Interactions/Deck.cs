using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Deck : MonoBehaviour
    {
        private SpriteRenderer sr;

        void Awake()
        {
            this.sr = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(DeckEventType type)
        {
            Sprite newSprite = null;
            switch (type)
            {
                case DeckEventType.Reset:
                    newSprite = Resources.LoadAll<Sprite>("cards").Single(x => x.name.Equals("cards_29"));
                    break;
                case DeckEventType.RocketStephenson:
                    newSprite = Resources.Load<Sprite>("RocketStephenson");
                    break;
                case DeckEventType.NathanRothschild:
                    newSprite = Resources.Load<Sprite>("NathanRothschild");
                    break;
                case DeckEventType.DeckEnd:
                    newSprite = null;
                    break;
            }

            sr.sprite = newSprite;
        }
    }
}
