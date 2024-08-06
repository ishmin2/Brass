using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.ActionBoard.UI;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket.Events;
using Assets.Scripts.Client.Interactions.SteelMarket;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Common;
using Assets.Scripts.Common.GameSaver.Models;
using Assets.Scripts.Common.Pathfind;
using Assets.Scripts.Common.PlayerActions;
using Assets.Scripts.Common.PlayerActions.Interfaces;
using Assets.Scripts.MainScene.Constants;
using Assets.Scripts.MainScene.Data;
using Assets.Scripts.MainScene.GameState;
using Assets.Scripts.MainScene.UI.PageElements;
using Assets.Scripts.MainScene.UI.PageObjects;
using Assets.Scripts.MenuScene;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.StaticObjects
{
    /*
     * Ответственности:
     *
     * 1) Подготовка стола к игре
     * 2) Передача хода между игроками
     * 3) Конец фазы раунда игрока
     * 4) Начало и завершение действия игрока
     */

    public class GameManager : MonoBehaviour, IEventHandler
    {
        [HideInInspector]
        public static GameManager i;
        public PlayerColors PlayerColor { get; set; }
        public string PlayerName { get; set; }
        public PlayerCard CurrentPlayerCard { get; set; }

        public IPlayerAction currentPlayerAction;

        public bool canTurn;

        private BuildingManager buildManager;
        private UIManager uiManager;
        private MapGraph mapGraph;
        private MoneyManager moneyManager;
        private CoalMarket coalMarket;
        private SteelMarket steelMarket;
        private MusicManager musicManager;

        public GameState GameState { get; private set; }

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

            GameState = new GameState();

            // DEBUG
            i.PlayerName = "Test";
            i.PlayerColor = Common.PlayerColors.Purple;
            

            DontDestroyOnLoad(this);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                var card = FindObjectOfType<DeckManager>().PopCard();
                FindObjectOfType<PlayerHand>().DrawCard(card);

                PlayersManager.i.AddPlayer(new Player
                {
                    Money = 10,
                    Color = PlayerColors.Purple,
                });

                this.GameState.ActivePlayer = PlayersManager.i.GetPlayer(PlayerColors.Purple);
            }
        }

        public void StartGame()
        {
            PlayersManager.i.AddPlayer(new Player { Name = "Shmin", Color = PlayerColors.Purple, PlayerType = PlayerType.Human }); // debug
            InitializePlayers();
            PlayerInfoPanel.i.Initialize();
            PlayerHand.i.Initialize(PlayersManager.i.GetPlayer(PlayerColor));
        }

        private void InitializePlayers()
        {
            foreach (var player in PlayersManager.i.Players)
            {
                for (int j = 0; j < 8; j++)
                {
                    var card = DeckManager.i.PopCard();
                    player.Value.Cards.Add(card);
                }

                player.Value.Money = 30;
                player.Value.IncomeTrackPosition = 10;
            }

            // TODO: random turn order
            this.GameState.ActivePlayer = PlayersManager.i.Players.First().Value;
        }

        public void DragCard(PlayerCard card)
        {
            this.CurrentPlayerCard = card;
            canTurn = true; // TODO: debug
            if (currentPlayerAction == null && canTurn)
            {
                UIManager.i.EnableActionsBoard();
            }
        }

        public void DropCard()
        {
            if (currentPlayerAction == null)
            {
                UIManager.i.DisableActionsBoard();
            }
        }

        public async void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is ValidationErrorActionEvent validationErrorEvent)
            {
                UIManager.i.ShowToast(validationErrorEvent.Message);
            }

            if (eventType is EnableCoalMarketActionEvent)

            {
                coalMarket.SetState(new CoalMarketTradeState(coalMarket));
            }

            if (eventType is DisableCoalMarketActionEvent)
            {
                coalMarket.SetState(new CoalMarketIdleState(coalMarket));
            }

            if (eventType is EnableSteelMarketActionEvent)
            {
                steelMarket.SetState(new SteelMarketTradeState(steelMarket));
            }

            if (eventType is DisableSteelMarketActionEvent)
            {
                steelMarket.SetState(new SteelMarketIdleState(steelMarket));
            }
        }

        public void StartPlayerAction(ActionType action)
        {
            UIManager.i.DisableActionsBoard();

            switch (action)
            {
                case ActionType.Build:
                    // currentPlayerAction = new BuildAction(steelMarket, coalMarket, uiManager, moneyManager, musicManager, mapGraph, pathfinder, this.CurrentPlayerCard);
                    break;
                case ActionType.Develop:
                    currentPlayerAction = new DevelopAction(this.buildManager, this.uiManager, this.mapGraph, this.moneyManager, this.musicManager);
                    break;
                case ActionType.Loan:
                    currentPlayerAction = new LoanAction(moneyManager, uiManager, musicManager);
                    break;
                case ActionType.PassTurn:
                    currentPlayerAction = new PassTurnAction(musicManager);
                    break;
                case ActionType.Road:
                    currentPlayerAction = new RoadAction(this.GameState.ActivePlayer);
                    break;
                case ActionType.Sell:
                    // currentPlayerAction = new SellAction(buildManager, this.cottonMarketManager, moneyManager, musicManager, pathfinder, mapGraph);
                    break;
            }

            var reason = string.Empty;
            if (!this.currentPlayerAction.CanStartAction(ref reason))
            {
                UIManager.i.ShowToast(reason);
                try
                {
                    this.currentPlayerAction.CancelAction();
                    foreach (var snapshotable in this.currentPlayerAction.Snapshotables)
                    {
                        snapshotable.Key.Restore(snapshotable.Value);
                    }

                    currentPlayerAction = null;
                }
                catch (CancelSellActionException)
                {
                    // HACK: with sell Action
                }
                return;
            }

            //this.currentPlayerAction.Snapshotables.Add(this.playerHand, this.playerHand.Save());
            // this.playerHand.MarkCardToDestroy(this.CurrentPlayerCard);
            UIManager.i.EnableApprovePanel();
            this.currentPlayerAction.StartAction();
        }

        public async void FinishAction()
        {
            string reason = string.Empty;
            if(currentPlayerAction == null || !currentPlayerAction.CanFinishAction(ref reason))
            {
                UIManager.i.ShowToast(reason);
                return;
            }

            currentPlayerAction = null;
            FindObjectOfType<PlayerHand>().CleanHand();
            UIManager.i.DisableApprovePanel();
            GameState.CurrentPlayerRemindActions--;

            if (GameState.CurrentPlayerRemindActions == 0)
            {
                // CurrentPlayer.Money += CurrentPlayer.Income;
                canTurn = false;
            }

            if (GameState.CurrentPlayerRemindActions == 0)
            {
                GameState.CurrentPlayerRemindActions = 2;

                UIManager.i.PlayEndTurnAnimation();
                musicManager.Play(SoundConstants.Yare_yare_daze);
                var card = FindObjectOfType<DeckManager>().PopCard();
                FindObjectOfType<PlayerHand>().DrawCard(card);
                await Task.Delay(200);
                card = FindObjectOfType<DeckManager>().PopCard();
                FindObjectOfType<PlayerHand>().DrawCard(card);

                // Container.GetLocalPlayer().CmdUserEndTurn();
            }
        }

        public void SetGameRound(int round)
        {
            GameState.GameRound = round;
            var obj = GameObject.FindGameObjectWithTag("RoundMarker");
            if (GameState.GameRound == 1)
            {
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round2")).gameObject
                    .GetComponent<Image>().enabled = false;
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round1")).gameObject
                    .GetComponent<Image>().enabled = true;
            }
            else
            {
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round2")).gameObject
                    .GetComponent<Image>().enabled = true;
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round1")).gameObject
                    .GetComponent<Image>().enabled = false;
            }
        }

        public static void LoadGameState(GameStateModel model)
        {
            Container.Instance().GetGameState().GameRound = model.GameRound;
            Container.Instance().GetGameState().CurrentPlayerRemindActions = model.RemindActionForPlayer;
            // Container.i().GetGameState().ActivePlayer = model.CurrentPlayer;
            // Container.GetLocalPlayer().RpcChangeTurnOrder(model.TurnOrder.ToArray());
        }

        public void RestrictLoan()
        {
            GameState.LoadAvailable = false;
        }
    }
}