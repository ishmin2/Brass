using System.Collections.Generic;
using Assets.Scripts.Client.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Server.Common
{
    public class ServerData
    {
        public int PlayersCount => this.PlayerPanels.Count;

        //public Dictionary<PlayerColors, NetworkConnection> PlayerConn { get; } = new Dictionary<PlayerColors, NetworkConnection>();
        
        public int? RemainRounds;

        public List<PlayerColors> PlayerTurnOrder = new List<PlayerColors>();

        public List<PlayerPanel> PlayerPanels = new List<PlayerPanel>();

        public int CurrentPlayer;

        public int Round = 1;

        public bool LoanRestricted;

        public int ActionsRemind;
    }
}
