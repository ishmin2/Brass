using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Data;
using Assets.Scripts.MainScene.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerHand : MonoBehaviour, IEventHandler, ISnapshotable
    {
        [HideInInspector]
        public static PlayerHand i;

        public List<PlayerCard> cards { get; set; }

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
        }

        void Start()
        {
            cards = new List<PlayerCard>();
            ClientEventAggregator.Subscribe(this);
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is DrawCardActionEvent drawCardEvent)
            {
                this.DrawCard(drawCardEvent.PlayerCard);
                this.StartCoroutine(this.ReturnDraggedCardMousePosition());
            }

            if (eventType is DropCardActionEvent)
            {
                // Rearrange card positions in hand. (need for case when card was dragging while draw new)
                foreach (var card in cards)
                {
                    card.transform.SetParent(null);
                    card.transform.SetParent(transform);
                }
            }
        }

        public void Initialize(Player player)
        {
            foreach (var playerCard in player.Cards)
            {
                AddCardIntoHand(playerCard);
            }
        }

        public async void DrawCard(PlayerCard playerCard)
        {
            await AnimationManager.instance.PlayerDrawCard();
            this.AddCardIntoHand(playerCard);
        }

        public void MarkCardToDestroy(PlayerCard card)
        {
            cards.Single(c => c.Equals(card)).gameObject.SetActive(false);
        }

        public void CleanHand()
        {
            var cardsToClean = new List<PlayerCard>();
            foreach (var card in cards.Where(c => !c.isActiveAndEnabled))
            {
                cardsToClean.Add(card);
                Destroy(card.gameObject);
            }

            foreach (var card in cardsToClean)
            {
                cards.Remove(card);
            }
        }

        private void AddCardIntoHand(PlayerCard playerCard)
        {
            var newCard = Instantiate(playerCard, transform);
            cards.Add(newCard);
        }

        private IEnumerator ReturnDraggedCardMousePosition()
        {
            // HACK: sorry me for that. When we add new card to panel, dragging card lose position. Here we return it back.
            // TODO: change parent to nothing when with start dragging
            //if (PlayerCard.DraggedCard != null)
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        PlayerCard.DraggedCard.GetComponent<PlayerCard>().OnDrag(null);
            //        yield return new WaitForSeconds(0.01f);
            //    }
            //}
            return null;
        }

        public ISnapshot Save()
        {
            return new PlayerHandSnapshot(this.AsModel());
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is PlayerHandSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = (PlayerHandSnapshot)snapshot;
            foreach (var playerCard in this.cards)
            {
                Destroy(playerCard.gameObject);
            }
            this.cards.Clear();

            var prefabs = Resources.LoadAll<PlayerCard>("Prefabs/DeckCards/2Players/");
            foreach (var snapCard in snapshotTyped.Cards)
            {
                this.cards.Add(Instantiate(prefabs.First(x => x.City == snapCard.City && x.Type == snapCard.BuildingType), this.transform));
            }
        }
    }
}
