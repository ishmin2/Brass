using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature.CottonFactory
{
    public class CottonFactoryActivatedState : State<Building>
    {
        public CottonFactoryActivatedState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            await BuildingHelper.ActivateBuilding(this.StateObject);
        }
    }
}
