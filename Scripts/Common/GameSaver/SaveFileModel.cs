using System;
using System.Collections.Generic;
using Assets.Scripts.Common.GameSaver.Models;

namespace Assets.Scripts.Common.GameSaver
{
    [Serializable]
    public class SaveFileModel
    {
        public List<DeckCardModel> DeckCards;

        public GameStateModel GameState;

        public MarketsModel Markets;

        public List<DashboardButtonModel> DashboardButtons;

        public List<PlayerModel> Player;

        public BoardStateModel Board;
    }
}
