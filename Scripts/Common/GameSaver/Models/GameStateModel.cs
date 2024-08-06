using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.GameSaver.Models
{
    [Serializable]
    public class GameStateModel
    {
        public int GameRound;

        public PlayerColors CurrentPlayer;

        public int RemindActionForPlayer;

        public List<PlayerColors> TurnOrder;
    }
}
