using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class BoardStateModel
    {
        public List<BuildingModel> Buildings;

        public List<RoadModel> Roads;
    }
}
