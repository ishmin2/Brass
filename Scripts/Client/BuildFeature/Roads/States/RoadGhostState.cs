using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.Roads.States
{
    public class RoadGhostState : State<Road>
    {
        private Sprite prevSprite;

        public RoadGhostState(Road stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            this.StateObject.PlayerColor = GameManager.i.GameState.ActivePlayer.Color;
            this.prevSprite = this.StateObject.SpriteRenderer.sprite;
            this.StateObject.SpriteRenderer.sprite = Resources.Load<Sprite>(new ResourcesPath(this.StateObject.PlayerColor).Railroad);
            this.StateObject.SpriteRenderer.color = new Color(1, 1, 1, 0.75f);
        }

        public override void OnStateExit()
        {
            this.StateObject.SpriteRenderer.sprite = this.prevSprite;
            this.StateObject.SpriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
}
