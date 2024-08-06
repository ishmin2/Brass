using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    public class BuildingSellResourceRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.BuildingSellResourceRequestModel;

        public CityNames City;

        public int BuildingSlot;
    }
}
