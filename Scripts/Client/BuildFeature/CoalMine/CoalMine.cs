using System.Threading.Tasks;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Interactions.CottonMarket.Events;
using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature.CoalMine
{
    public class CoalMineActions
    {
        public async Task Sell(State<Building> state)
        {
            state.OnClickAction = null;
            state.StateObject.CoalToSellRemind--;
            BuildingHelper.HighlightGameObject(state.StateObject.gameObject, false);
            state.StateObject.ResourcesAmount.SetNumber(state.StateObject.CoalToSellRemind);

            // HACK: выключаем все шахты пока ждем кучу анимаций. Нужно менять, это полный бред
            Container.GetBuildingManager().DeactivateBuildingByFactoryType(BuildingType.CoalMine);
            Container.GetBuildingManager().DeactivateBuildingByFactoryType(BuildingType.WorldTrade);
            ClientEventAggregator.Publish(new DisableCoalMarketActionEvent());

            Container.GetMusicManager().Play(SoundConstants.Coin);
            await BuildingHelper.AnimateBuildingSell(state.StateObject, "Prefabs/Materials/Coal"); 
            if (state.StateObject.CoalToSellRemind > 0)
            {
                state.StateObject.SetIdleState();
            }
            else
            {
                await state.StateObject.SetFinishStateAsync();
            }

            ClientEventAggregator.Publish(new BuildingSellResourceRequestModel { City = BuildingExtension.GetCity(state.StateObject).Name, BuildingSlot = state.StateObject.CitySlotNumber });
        }

        public async Task RpcSell(State<Building> state)
        {
            state.StateObject.CoalToSellRemind--;
            await BuildingHelper.AnimateBuildingSell(state.StateObject, "Prefabs/Materials/Coal");
            state.StateObject.ResourcesAmount.SetNumber(state.StateObject.CoalToSellRemind);

            if (state.StateObject.CoalToSellRemind > 0)
            {
                state.StateObject.SetIdleState();
            }
            else
            {
                await state.StateObject.SetFinishStateAsync();
            }

            ClientEventAggregator.Publish(new BuildingSellResourceRequestModel { City = BuildingExtension.GetCity(state.StateObject).Name, BuildingSlot = state.StateObject.CitySlotNumber });
        }
    }
}
