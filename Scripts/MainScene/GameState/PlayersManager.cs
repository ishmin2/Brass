using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Data;
using UnityEngine;

namespace Assets.Scripts.MainScene.GameState
{
    public class PlayersManager : MonoBehaviour
    {
        [HideInInspector]
        public static PlayersManager i;

        public int PlayerCount => players.Count;

        public IReadOnlyDictionary<PlayerColors, Player> Players => players;

        private Dictionary<PlayerColors, Player> players;

        void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else if (i == this)
            {
                Destroy(gameObject);
            }

            this.players = new Dictionary<PlayerColors, Player>();
            DontDestroyOnLoad(this);
        }

        public void AddPlayer(Player player)
        {
            players.Add(player.Color, player);
        }

        public Player GetPlayer(PlayerColors color)
        {
            if (IsPlayerExist(color))
            {
                return players[color];
            }

            throw new Exception($"Player {color} not found in game");
        }

        public bool IsPlayerExist(PlayerColors color)
        {
            return players.ContainsKey(color);
        }

        public PlayerColors[] GetFreePlayerColor()
        {
            var freeColors = Enum.GetValues(typeof(PlayerColors))
                .OfType<PlayerColors>()
                .Where(color => color != PlayerColors.None)
                .Where(color => !players.Keys.Contains(color))
                .ToArray();
            return freeColors;
        }
    }
}
