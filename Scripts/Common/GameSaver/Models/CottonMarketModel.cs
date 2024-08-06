using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class CottonMarketModel
    {
        public int TrackPosition;
        public int? LastCard;
        public List<int> DeckCards;
    }
}
