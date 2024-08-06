using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature.Harbor
{
    public class HarborActivatedState : State<Building>
    {
        public HarborActivatedState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            await BuildingHelper.ActivateBuilding(this.StateObject);
        }
    }
}
