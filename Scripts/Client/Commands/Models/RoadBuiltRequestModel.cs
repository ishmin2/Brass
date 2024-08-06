using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    [Serializable]
    public class RoadBuiltRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.RoadBuiltRequestModel;

        public CityNames CityA;

        public CityNames CityB;

        public int ownerID;
    }
}
