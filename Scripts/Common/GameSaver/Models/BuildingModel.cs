using System;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class BuildingModel
    {
        public CityNames City;
        public int CitySlotNumber;
        public BuildingType BuildingType;
        public int BuildingLevel;
        public PlayerColors PlayerColor;
        public int CoalToSellRemind;
        public int SteelToSellRemind;
        public bool isActivated;
        public bool CanWorldTrade;
        public int VictoryPoints;
        public int IncomeValue;
    }
}