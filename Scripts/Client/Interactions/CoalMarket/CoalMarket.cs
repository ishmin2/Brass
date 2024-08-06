using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.PlayerInput;
using Assets.Scripts.Common;
using Assets.Scripts.Common.GameSaver.Models;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CoalMarket
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CoalMarket : MonoBehaviour, IClickable, ISnapshotable, IMarket
    {
        [HideInInspector]
        public static CoalMarket instance;

        public State<CoalMarket> currentState { get; protected set; }

        public int CurrentCost => this.currentPosition / 2 + 2;

        public int AvailableCoal => this.tradeObjects.Count - this.currentPosition;

        private List<GameObject> tradeObjects;
        private int currentPosition;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            this.tradeObjects = this.GetComponentsInChildren<Transform>().Skip(1).Select(x => x.gameObject).ToList();
            currentState = new CoalMarketIdleState(this);
        }

        public void SetState(State<CoalMarket> state)
        {
            if (currentState != null)
            {
                currentState.OnStateExit();
            }

            currentState = state;

            if (currentState != null)
            {
                currentState.OnStateEnter();
            }
        }

        public void OnClick()
        {
            currentState?.OnClickAction?.Invoke();
        }

        public int Buy()
        {
            int cost;
            if (this.currentPosition < this.tradeObjects.Count)
            {
                this.tradeObjects[this.currentPosition].SetActive(false);
                cost = (this.currentPosition / 2) + 2;
                this.currentPosition++;
            }
            else
            {
                cost = 5;
            }

            ClientEventAggregator.Publish(new MarketBuyResourceRequestModel(MarketType.CoalMarket, cost));
            return cost;
        }

        public int Sell()
        {
            var cost = 0;

            if (this.currentPosition == this.tradeObjects.Count || this.currentPosition > 0)
            {
                this.currentPosition--;
            }

            this.tradeObjects[this.currentPosition].SetActive(true);
            cost += (this.currentPosition / 2) + 2;

            return cost;
        }

        public int AvailableSellCount()
        {
            return this.currentPosition;
        }

        public void SetAvailableCount(int count)
        {
            for (int i = 0; i < this.tradeObjects.Count - count; i++)
            {
                this.tradeObjects[i].SetActive(false);
            }

            for (int i = this.tradeObjects.Count - count; i < this.tradeObjects.Count; i++)
            {
                this.tradeObjects[i].SetActive(true);
            }

            this.currentPosition = this.tradeObjects.Count - count;
        }

        public ISnapshot Save()
        {
            return new CoalMarketSnapshot(new CoalMarketModel {AvailableResources = AvailableCoal});
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is CoalMarketSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = (CoalMarketSnapshot)snapshot;
            this.SetAvailableCount(snapshotTyped.Model.AvailableResources);
            this.SetState(new CoalMarketIdleState(this));
        }
    }
}
