using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature.CottonFactory
{
    public class CottonFactoryFinishState : State<Building>
    {
        public CottonFactoryFinishState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            // StateObject.PlayerColor = StateObject.PlayerColor == 0 ? Container.GetLocalPlayer().PlayerColor : StateObject.PlayerColor;
        }
    }
}
