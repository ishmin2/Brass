using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Data;

namespace Assets.Scripts.Client
{
    public class GameState
    {
        public int GameRound;

        public Player ActivePlayer;

        public int CurrentPlayerRemindActions;

        public List<PlayerColors> TurnOrder;

        public bool LoadAvailable;

        public GameState()
        {
            this.GameRound = 1;
            this.LoadAvailable = true;
            this.TurnOrder = new List<PlayerColors>();
        }
    }
}
