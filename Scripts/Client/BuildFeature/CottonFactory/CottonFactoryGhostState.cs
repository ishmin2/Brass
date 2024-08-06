using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature.CottonFactory
{
    public class CottonFactoryGhostState : State<Building>
    {
        private SpriteRenderer sr;

        public CottonFactoryGhostState(Building stateObject) : base(stateObject)
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

        public override Task OnStateEnterAsync()
        {
            this.OnStateEnter();
            return Task.FromResult(true);
        }

        public override Task OnStateExitAsync()
        {
            this.OnStateExit();
            return Task.FromResult(true);
        }
    }
}
