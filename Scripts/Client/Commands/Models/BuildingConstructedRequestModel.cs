using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    public class BuildingConstructedRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.BuildingConstructedRequestModel;

        public CityNames City;

        public int BuildingSlot;

        public bool IsRebuild;

        public BuildingType BuildingType;

        public int BuildingLevel;

        public PlayerColors PlayerColor;

        public int SteelToSellRemind;

        public int CoalToSellRemind;
    }
}
