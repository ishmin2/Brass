using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    [Serializable]
    public class ActivateBuildingRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.ActivateBuildingRequestModel;

        public CityNames City;

        public int CitySlotNumber;
    }
}
