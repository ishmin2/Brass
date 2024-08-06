using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Client;
using Assets.Scripts.Common.GameSaver.Models;
using Assets.Scripts.Server;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.GameSaver
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveFileModel save;

        void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(this.Save);
        }

        public void Save()
        {
            //var cards = ServerContainer.i().DeckManager.Value.Deck;
            //var deckCards = cards.Select(x => new DeckCardModel { City = x.City, BuildingType = x.Type }).ToList();
            //var dashboardButtons = Container.GetDashboardManager().columns
            //    .SelectMany(x => x.Buttons)
            //    .Select(b => new DashboardButtonModel { Level = b.ButtonLevel, RemindCount = b.DashboardButtonCounter.Count, BuildingType = b.BuildingType })
            //    .ToList();

            var mapGraph = Container.GetMapGraph();
            var buildings = mapGraph.GetCities()
                .SelectMany(c => c.Buildings())
                .Where(b => b.PlayerColor != PlayerColors.None)
                .Select(b => b.AsModel());
            var roads = mapGraph.GetAllRoads()
                .Where(r => r.PlayerColor != PlayerColors.None)
                .Select(r => r.AsModel());
            
            var saveFile = new SaveFileModel
            {
                //DeckCards = deckCards,
                GameState = new GameStateModel
                {
                    GameRound = Container.Instance().GetGameState().GameRound,
                    RemindActionForPlayer = Container.Instance().GetGameState().CurrentPlayerRemindActions,
                    // CurrentPlayer = Container.i().GetGameState().ActivePlayer,
                    TurnOrder = Container.Instance().GetGameState().TurnOrder,
                },
                Markets = new MarketsModel
                {
                    CottonMarket = new CottonMarketModel
                    {
                        TrackPosition = Container.GetCottonMarket().currentCost,
                        LastCard = Container.GetCottonMarket().lastPlayedCard?.Cost,
                        DeckCards = ServerContainer.Instance().OuterMarketManager.Value.Deck.Select(c => c.Cost).ToList() // TODO: Get value by [COMMAND] from Server
                    }, 
                    CoalMarket = Container.GetCoalMarket().AsModel(),
                    SteelMarket = Container.GetSteelMarket().AsModel(),
                },
                //DashboardButtons = dashboardButtons,
                Player = new List<PlayerModel>
                {
                    // TODO: // TODO: Get value by [COMMAND] from Server for all players
                    new PlayerModel
                    {
                        PlayerHand = Container.GetPlayerHand().AsModel(),
                        PlayerPanelModel = Container.Instance().GetPlayerPanel().AsModel(),
                        // PlayerColor = Container.GetLocalPlayer().PlayerColor,
                    },
                },
                Board = new BoardStateModel
                {
                    Buildings = new List<BuildingModel>(buildings),
                    Roads = new List<RoadModel>(roads)
                }
            };

            var saveFileJson = JsonUtility.ToJson(saveFile, true);
            save = saveFile;

            File.WriteAllText(this.GetSavePath(), saveFileJson);
            Debug.Log("Game saved");
        }

        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, Random.Range(1, 100) + ".sav");
        }
    }
}

