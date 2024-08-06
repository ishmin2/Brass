using Assets.Scripts.Client.BuildFeature;
using UnityEngine;

namespace Assets.Scripts.Client.Interactions.SteelMarket
{
    public class SteelMarketTradeState : State<SteelMarket>
    {
        public SteelMarketTradeState(SteelMarket stateObject) : base(stateObject)
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
