using Assets.Scripts.Client.BuildFeature;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CoalMarket
{
    public class CoalMarketIdleState : State<CoalMarket>
    {
        public CoalMarketIdleState(CoalMarket stateObject) : base(stateObject)
        {

        }

        public override void OnStateEnter()
        {
            this.StateObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
