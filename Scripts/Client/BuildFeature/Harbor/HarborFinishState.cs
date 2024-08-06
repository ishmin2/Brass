using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature.Harbor
{
    public class HarborFinishState : State<Building>
    {
        public HarborFinishState(Building stateObject) : base(stateObject)
        {
        }

        public override async Task OnStateEnterAsync()
        {
            // StateObject.PlayerColor = StateObject.PlayerColor == 0 ? Container.GetLocalPlayer().PlayerColor : StateObject.PlayerColor;
        }
    }
}
