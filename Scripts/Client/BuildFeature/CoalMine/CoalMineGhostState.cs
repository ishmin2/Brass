using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.CoalMine
{
    public class CoalMineGhostState : State<Building>
    {
        private SpriteRenderer sr;

        public CoalMineGhostState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            this.sr = StateObject.GetComponent<SpriteRenderer>();
            this.sr.color = new Color(1, 1, 1, 0.75f);
        }

        public override void OnStateExit()
        {
            this.sr.color = new Color(1, 1, 1, 1);
        }

        public override async Task OnStateEnterAsync()
        {
            this.OnStateEnter();
        }

        public override async Task OnStateExitAsync()
        {
            this.OnStateExit();
        }
    }
}
