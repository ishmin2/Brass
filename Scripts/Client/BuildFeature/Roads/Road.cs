using System;
using Assets.Scripts.Client.BuildFeature.Roads.States;
using Assets.Scripts.Client.PlayerInput;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.Roads
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Road : MonoBehaviour, IClickable, ISnapshotable
    {
        public City CityA;
        public City CityB;

        public bool isRiverAvailable;
        public PlayerColors PlayerColor;

        public State<Road> currentState { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        void Awake()
        {
            SpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            currentState?.Tick();
        }

        public void OnClick()
        {
            currentState?.OnClickAction?.Invoke();
        }

        public void SetState(State<Road> state)
        {
            currentState?.OnStateExit();
            currentState = state;
            currentState?.OnStateEnter();
        }

        public ISnapshot Save()
        {
            return new RoadSnapshot(this.AsModel());
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is RoadSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = (RoadSnapshot)snapshot;
            this.PlayerColor = snapshotTyped.Model.PlayerColor;

            if (this.PlayerColor == PlayerColors.None)
            {
                this.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                var spritePath = Container.Instance().GetGameState().GameRound == 1
                    ? new ResourcesPath(this.PlayerColor).Boat
                    : new ResourcesPath(this.PlayerColor).Railroad;
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
            }
            
            this.SetState(new RoadIdleState(this));
        }

        public void Reset()
        {
            this.PlayerColor = PlayerColors.None;
            this.SpriteRenderer.sprite = null;
        }
    }
}
