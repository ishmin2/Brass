using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CottonMarket
{
    public class CottonMarketManager : MonoBehaviour, IEventHandler
    {
        public GameObject Marker;
        public Transform DeckPosition;
        public Transform DeckDiscardPosition;
        public int CardSpeed;
        public int MarkerSpeed;
        public CottonMarketCard currentCard;
        public CottonMarketCard lastPlayedCard;

        public int currentCost { get; private set; }
        public int destinationCost { get; private set; }

        private List<Vector3> marketWaypoint;

        void Start()
        {
            this.marketWaypoint = this.Marker.GetComponentsInChildren<Transform>()
                .Skip(1)
                .Select(x => x.position)
                .ToList();

            ClientEventAggregator.Subscribe(this);
        }

        public bool CanTrade()
        {
            if (currentCost == this.marketWaypoint.Count - 1)
            {
                return false;
            }

            return true;
        }

        public int GetSellBonus()
        {
            var res = 3 - (destinationCost / 2);
            return res > 0 ? res : 0;
        }

        public async Task DrawCard(CottonMarketCard card)
        {
            this.currentCard = Instantiate(card, this.DeckPosition);
            
            this.SubstructMarketCost(card.Cost);
            await this.MoveCardToDiscard();
            await this.MoveMarketToTargetCost();

            if (this.lastPlayedCard != null && this.lastPlayedCard?.gameObject != null)
            {
                Destroy(this.lastPlayedCard.gameObject);
            }

            this.lastPlayedCard = this.currentCard;
            ClientEventAggregator.Publish(new CottonCardDrawnEvent());
        }

        public void Reset()
        {
            this.currentCost = 0;
            this.destinationCost = 0;
            this.Marker.transform.position = this.marketWaypoint[0];

            if (this.lastPlayedCard?.gameObject != null)
            {
                Destroy(this.lastPlayedCard.gameObject);
            }
        }

        public void SetTrackValue(int trackValue)
        {
            this.Marker.transform.position = this.marketWaypoint[trackValue];
            currentCost = trackValue;
        }

        public void SetDiscardCard(CottonMarketCard card)
        {
            this.lastPlayedCard = card;
            card.transform.position = this.DeckDiscardPosition.position;
        }

        private void SubstructMarketCost(int count)
        {
            count = Math.Abs(count);
            if (count >= (this.marketWaypoint.Count - 1) - currentCost)
            {
                destinationCost = (this.marketWaypoint.Count - 1);
            }
            else
            {
                destinationCost = currentCost + count;
            }
        }

        private async Task MoveMarketToTargetCost()
        {
            var from = this.Marker.transform.position;
            var to = this.marketWaypoint[currentCost];

            while(Vector3.Distance(from, this.marketWaypoint[destinationCost]) > 0.01f)
            {
                if (Vector3.Distance(from, to) < 0.01f)
                {
                    currentCost++;
                    to = this.marketWaypoint[currentCost];
                }
                else
                {
                    this.Marker.transform.position = Vector3.MoveTowards(from, to, Time.deltaTime * this.MarkerSpeed);
                    from = this.Marker.transform.position;
                }

                await Task.Delay(2);
            }
        }

        private async Task MoveCardToDiscard()
        {
            var to = this.DeckDiscardPosition.position;
            while (true)
            {
                var from = this.currentCard.gameObject.transform.position;

                if (this.currentCard.gameObject.transform.eulerAngles.y < 180)
                {
                    this.currentCard.gameObject.transform.Rotate(0, Time.deltaTime * this.CardSpeed * 10, 0);
                    continue;
                }

                if (Vector3.Distance(from, to) > 0.01f)
                {
                    this.currentCard.gameObject.transform.position = Vector3.MoveTowards(from, to, Time.deltaTime * this.CardSpeed);
                }
                else
                {
                    this.currentCard.gameObject.transform.position = new Vector3(this.currentCard.gameObject.transform.position.x, this.currentCard.gameObject.transform.position.y, this.DeckDiscardPosition.position.z);
                    this.DeckDiscardPosition.position -= Vector3.forward;
                    break;
                }

                await Task.Delay(20);
            }
        }

        public async void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is DrawOuterMarketCardEvent drawOuterMarketCardEvent)
            {
                await this.DrawCard(drawOuterMarketCardEvent.CottonMarketCard);
            }
        }
    }
}
