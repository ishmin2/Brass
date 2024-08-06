using Assets.Scripts.Client.BuildFeature;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.SteelMarket
{
    public class SteelMarketIdleState : State<SteelMarket>
    {
        public SteelMarketIdleState(SteelMarket stateObject) : base(stateObject)
        {

        }

        public override void OnStateEnter()
        {
            this.StateObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
