namespace Assets.Scripts.Client.BuildFeature.CoalMine
{
    public class CoalMineSellState : State<Building>
    {
        public CoalMineSellState(Building stateObject) : base(stateObject)
        {
        }

        public override void OnStateEnter()
        {
            if (this.StateObject.CoalToSellRemind == 0)
            {
                this.StateObject.SetState(new CoalMineIdleState(StateObject));
                return;
            }

            BuildingHelper.HighlightGameObject(StateObject.gameObject, true);
            OnClickAction = async () => await this.StateObject.CoalMine.Sell(this);
        }

        public override void OnStateExit()
        {
            BuildingHelper.HighlightGameObject(StateObject.gameObject, false);
        }
    }
}
