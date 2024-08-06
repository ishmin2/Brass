using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    public class CardPlayedRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.CardPlayedRequestModel;

        public PlayerColors PlayerColor;

        public CityNames City;

        public BuildingType BuildingType;

        public CardPlayedRequestModel(CityNames cityName, BuildingType buildingType, PlayerColors playerColor)
        {
            this.City = cityName;
            this.BuildingType = buildingType;
            this.PlayerColor = playerColor;
        }
    }
}
