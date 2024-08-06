using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class PlayerModel
    {
        public PlayerColors PlayerColor;
        public PlayerPanelModel PlayerPanelModel;
        public List<DeckCardModel> PlayerHand;
    }
}
