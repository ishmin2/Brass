using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature.Shipyard
{
    public class ShipyardFinishState : State<Building>
    {
        public ShipyardFinishState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            await BuildingHelper.ActivateBuilding(this.StateObject);
        }
    }
}
