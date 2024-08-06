using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.GameFolder.Dashboard;
using Assets.Scripts.Client.Interactions;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Client.UI;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Pathfind;
using Assets.Scripts.MainScene.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client
{
    public class Container
    {
        private GameState gameState;
        private Deck deck;
        private static Container _container;

        private Container() { }

        public static Container Instance()
        {
            if (_container == null)
            {
                _container = new Container();
            }

            return _container;
        }

        public GameState GetGameState()
        {
            if (this.gameState == null)
            {
                this.gameState = new GameState();
            }

            return this.gameState;
        }

        public PlayerPanel GetPlayerPanel()
        {
            return null; // GetPlayerPanels().Single(pp => pp.Color == GetLocalPlayer().PlayerColor);
        }

        public Deck GetDeck()
        {
            if (this.deck == null)
            {
                this.deck = GameObject.FindGameObjectWithTag("Board").GetComponentInChildren<Deck>();
            }

            return this.deck;
        }

        public static Player GetLocalPlayer()
        {
            return null;
            // return Object.FindObjectsOfType<Player>().Single(x => x.isClient && x.isLocalPlayer);
        }

        public static MusicManager GetMusicManager()
        {
            return GameObject.Find("GameManager").GetComponent<MusicManager>();
        }

        public static BuildingManager GetBuildingManager()
        {
            return GameObject.Find("GameManager").GetComponent<BuildingManager>();
        }

        public static MoneyManager GetMoneyManager()
        {
            return GameObject.Find("Canvas").GetComponent<MoneyManager>();
        }

        public static GameObject GetCanvas()
        {
            return GameObject.Find("Canvas");
        }

        public static CoalMarket GetCoalMarket()
        {
            return GameObject.Find("CoalMarket").GetComponent<CoalMarket>();
        }

        public static SteelMarket GetSteelMarket()
        {
            return GameObject.Find("SteelMarket").GetComponent<SteelMarket>();
        }
        
        public static Text GetValidationErrorText()
        {
            return GameObject.FindGameObjectWithTag("ValidationError").GetComponent<Text>();
        }

        public static PlayerHand GetPlayerHand()
        {
            return GameObject.Find("PlayerHand").GetComponent<PlayerHand>();
        }

        public static CottonMarketManager GetCottonMarket()
        {
            return GameObject.Find("OuterMarket").GetComponent<CottonMarketManager>();
        }

        public static DashboardManager GetDashboardManager()
        {
            return GameObject.Find("Canvas").GetComponent<UIManager>().BuildingsDashboard;
        }

        public static MapGraph GetMapGraph()
        {
            return GameObject.Find("Board").GetComponent<MapGraph>();
        }

        public static List<PlayerPanel> GetPlayerPanels()
        {
            return GameObject.Find("PlayerPanel").GetComponentsInChildren<PlayerPanel>().ToList();
        }
    }
}
