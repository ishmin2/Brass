using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature
{
    interface IBuilding
    {
        void SetSellState();

        void SetIdleState();

        void SetFinishState();

        void SetUpgradeState();

        Task SetSellStateAsync();

        Task SetIdleStateAsync();

        Task SetFinishStateAsync();

        Task SetGhostStateAsync();

        Task SetUpgradeStateAsync();
    }
}
