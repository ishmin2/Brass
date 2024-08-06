using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.Roads.States
{
    public class RoadBuiltState : State<Road>
    {
        private readonly PlayerColors playerColor;

        public RoadBuiltState(Road stateObject, PlayerColors playerColor) : base(stateObject)
        {
            this.playerColor = playerColor;
        }

        public override void OnStateEnter()
        {
            var path = new ResourcesPath(this.playerColor);
            var spriteName = path.Boat;
            if (GameManager.i.GameState.GameRound == 2)
            {
                spriteName = path.Railroad;
            }
            
            this.StateObject.SpriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
            this.StateObject.PlayerColor = this.playerColor;
        }
    }
}
