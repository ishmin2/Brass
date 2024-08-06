using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Data;
using Assets.Scripts.MainScene.GameState;
using Assets.Scripts.MainScene.UI.PageElements;
using UnityEngine;

namespace Assets.Scripts.MainScene.UI.PageObjects
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        [HideInInspector]
        public static PlayerInfoPanel i;

        private Dictionary<PlayerColors, PlayerInfo> PlayerInfos;

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

            PlayerInfos = new Dictionary<PlayerColors, PlayerInfo>();
        }

        public void Initialize()
        {
            foreach (var player in PlayersManager.i.Players)
            {
                var playerInfoPrefab = Resources.Load<PlayerInfo>(ResourcesPath.PlayerInfoElement);
                var playerInfo = Instantiate(playerInfoPrefab, this.gameObject.transform);

                playerInfo.Avatar.sprite = Resources.Load<Sprite>(new ResourcesPath(player.Key).Avatar);
                playerInfo.PlayerColor = player.Key;
                playerInfo.CardCountText.text = player.Value.Cards.Count.ToString();
                playerInfo.MoneyText.text = player.Value.Money.ToString();
                playerInfo.IncomeText.text = player.Value.Income.ToString();
                playerInfo.MoneySpentText.text = "0";

                PlayerInfos.Add(player.Key, playerInfo);
            }
        }
    }
}