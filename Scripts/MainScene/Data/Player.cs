using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Constants;

namespace Assets.Scripts.MainScene.Data
{
    public class Player
    {
        public string Name { get; set; }
        public PlayerColors Color { get; set; }
        public List<PlayerCard> Cards { get; set; }
        public int Money { get; set; }
        public int Income { get; set; }
        public int IncomeTrackPosition { get; set; }
        public int VictoryPoints { get; set; }
        public int MoneySpentInActionRound { get; set; }
        public PlayerType PlayerType { get; set; }

        public Player()
        {
            Cards = new List<PlayerCard>();
        }
    }
}
