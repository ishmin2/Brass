
using System;
using System.Threading.Tasks;
using Assets.Scripts.Client.BuildFeature.CoalMine;
using Assets.Scripts.Client.BuildFeature.CottonFactory;
using Assets.Scripts.Client.BuildFeature.DefaultBuilding;
using Assets.Scripts.Client.BuildFeature.Harbor;
using Assets.Scripts.Client.BuildFeature.Shipyard;
using Assets.Scripts.Client.BuildFeature.SteelFactory;
using Assets.Scripts.Client.BuildFeature.WorldTrade;
using Assets.Scripts.Client.PlayerInput;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature
{
    [Serializable]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : MonoBehaviour, IClickable, IBuilding, ISnapshotable
    {
        public BuildingType[] AvailableBuildingType;

        public int CitySlotNumber;

        public BuildingType BuildingType;

        public BuildPrize price;

        public int CoalToSellRemind;

        public int SteelToSellRemind;

        public int IncomeValue;

        public int VictoryPoints;

        [HideInInspector]
        public bool isActivated;
        public bool CanWorldTrade;

        public int Level;

        public PlayerColors PlayerColor;

        public ResourceBox ResourcesAmount;

        public State<Building> currentState { get; protected set; }

        public CoalMineActions CoalMine { get; } = new CoalMineActions();

        void Awake()
        {
            this.SetIdleState();
        }

        void Update()
        {
            currentState?.Tick();
        }

        public void OnClick()
        {
            currentState.OnClickAction?.Invoke();
        }

        public void SetState(State<Building> state)
        {
            currentState?.OnStateExit();

            currentState = state;

            currentState?.OnStateEnter();
        }

        public async Task SetStateAsync(State<Building> state)
        {
            await currentState.OnStateExitAsync();
            currentState = state;
            await currentState.OnStateEnterAsync();
        }

        public void SetSellState()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineSellState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactorySellState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactorySellState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborSellState(this);
                    break;
                case BuildingType.Shipyard:
                    break;
                case BuildingType.WorldTrade:
                    state = new WorldTradeSellState(this);
                    break;
            }

            this.SetState(state);
        }

        public void SetIdleState()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineIdleState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryIdleState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryIdleState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborIdleState(this);
                    break;
                case BuildingType.Shipyard:
                    state = new ShipyardIdleState(this);
                    break;
                case BuildingType.WorldTrade:
                    state = new WorldTradeIdleState(this);
                    break;
                case BuildingType.None:
                    state = new BuildingIdleState(this);
                    break;
            }

            this.SetState(state);
        }

        public void SetGhostState()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineGhostState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryGhostState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryGhostState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborGhostState(this);
                    break;
                case BuildingType.Shipyard:
                    state = new ShipyardGhostState(this);
                    break;
            }

            this.SetState(state);
        }

        public void SetFinishState()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineFinishState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryFinishState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryFinishState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborFinishState(this);
                    break;
                case BuildingType.Shipyard:
                    break;
                case BuildingType.WorldTrade:
                    state = new WorldTradeIdleState(this);
                    break;
            }

            this.SetState(state);
        }

        public void SetUpgradeState()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineUpgradeState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryUpgradeState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryUpgradeState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborUpgradeState(this);
                    break;
                case BuildingType.Shipyard:
                    state = new ShipyardUpgradeState(this);
                    break;
            }

            this.SetState(state);
        }

        public async Task SetSellStateAsync()
        {
            this.SetSellState();
        }

        public async Task SetIdleStateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetGhostStateAsync()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineGhostState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryGhostState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryGhostState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborGhostState(this);
                    break;
                case BuildingType.Shipyard:
                    state = new ShipyardGhostState(this);
                    break;
            }

            await this.SetStateAsync(state);
        }

        public async Task SetUpgradeStateAsync()
        {
            this.SetUpgradeState();
        }

        public async Task SetFinishStateAsync()
        {
            State<Building> state = null;
            switch (this.BuildingType)
            {
                case BuildingType.CoalMine:
                    state = new CoalMineFinishState(this);
                    break;
                case BuildingType.CottonFactory:
                    state = new CottonFactoryFinishState(this);
                    break;
                case BuildingType.SteelFactory:
                    state = new SteelFactoryFinishState(this);
                    break;
                case BuildingType.Harbor:
                    state = new HarborFinishState(this);
                    break;
                case BuildingType.Shipyard:
                    state = new ShipyardFinishState(this);
                    break;
                case BuildingType.WorldTrade:
                    state = new WorldTradeIdleState(this);
                    break;
            }

            await this.SetStateAsync(state);
        }

        public void UpdateBuilding(Building building, PlayerColors ownerColor)
        {
            this.BuildingType = building.BuildingType;
            this.price = new BuildPrize
            {
                CoalToBuildRemind = building.price.CoalToBuildRemind,
                SteelToBuildRemind = building.price.SteelToBuildRemind,
                MoneyCost = building.price.MoneyCost
            };

            this.CoalToSellRemind = building.CoalToSellRemind;
            this.SteelToSellRemind = building.SteelToSellRemind;
            this.IncomeValue = building.IncomeValue;
            this.VictoryPoints = building.VictoryPoints;
            this.CanWorldTrade = building.CanWorldTrade;
            this.PlayerColor = ownerColor;
            this.Level = building.Level;

            this.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"AllSprites/{ownerColor}/{building.BuildingType}_def_{building.Level}");
        }

        public ISnapshot Save()
        {
            return new BuildingSnapshot(this.AsModel());
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is BuildingSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = (BuildingSnapshot)snapshot;

            if (this.ResourcesAmount != null)
            {
                Destroy(this.ResourcesAmount.gameObject);
                this.ResourcesAmount = null;
            }

            this.CoalToSellRemind = snapshotTyped.Model.CoalToSellRemind;
            this.SteelToSellRemind = snapshotTyped.Model.SteelToSellRemind;
            this.PlayerColor = snapshotTyped.Model.PlayerColor;
            this.isActivated = snapshotTyped.Model.isActivated;
            this.Level = snapshotTyped.Model.BuildingLevel;
            this.BuildingType = snapshotTyped.Model.BuildingType;
            this.CanWorldTrade = snapshotTyped.Model.CanWorldTrade;
            this.VictoryPoints = snapshotTyped.Model.VictoryPoints;
            this.IncomeValue = snapshotTyped.Model.IncomeValue;

            var state = snapshotTyped.Model.isActivated ? BuildingSpriteStates.Activated : BuildingSpriteStates.Default;

            this.GetComponent<SpriteRenderer>().sprite = this.PlayerColor == PlayerColors.None ?
                null :
                Resources.Load<Sprite>($"AllSprites/{this.PlayerColor}/{this.BuildingType}_{state}_{this.Level}");
            this.SetIdleState();

            if (this.CoalToSellRemind > 0)
            {
                if (this.ResourcesAmount == null)
                {
                    var resource = Resources.Load<ResourceBox>("Prefabs/Factories/CoalBox");
                    this.ResourcesAmount = Instantiate(resource, transform);
                }

                this.ResourcesAmount.SetNumber(this.CoalToSellRemind);
            }
            else if (this.SteelToSellRemind > 0)
            {
                if (this.ResourcesAmount == null)
                {
                    var resource = Resources.Load<ResourceBox>("Prefabs/Factories/SteelBox");
                    this.ResourcesAmount = Instantiate(resource, transform);
                }

                this.ResourcesAmount.SetNumber(this.SteelToSellRemind);
            }
        }

        public void Reset()
        {
            this.PlayerColor = PlayerColors.None;
            this.BuildingType = BuildingType.None;
            this.CoalToSellRemind = 0;
            this.SteelToSellRemind = 0;
            this.isActivated = false;
            this.GetComponent<SpriteRenderer>().sprite = null;

            if (this.ResourcesAmount != null)
            {
                Destroy(this.ResourcesAmount.gameObject);
                this.ResourcesAmount = null;
            }
        }
    }
}
