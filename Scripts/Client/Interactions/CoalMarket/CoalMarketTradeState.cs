using Assets.Scripts.Client.BuildFeature;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.CoalMarket
{
    public class CoalMarketTradeState : State<CoalMarket>
    {
        public CoalMarketTradeState(CoalMarket stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            var box = Resources.Load<Sprite>("SelectionBox-72x72");
            this.StateObject.GetComponent<SpriteRenderer>().sprite = box;

            OnClickAction = () =>
            {
                this.StateObject.Buy();
            };

        }

        public override void OnStateExit()
        {
            this.StateObject.GetComponent<SpriteRenderer>().sprite = null;
            OnClickAction = null;
        }
    }
}
