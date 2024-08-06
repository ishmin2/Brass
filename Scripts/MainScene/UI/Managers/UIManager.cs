using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.GameFolder.Dashboard;
using Assets.Scripts.Client.MoneyController;
using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Client.UI.Score;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIManager : MonoBehaviour, IEventHandler
    {
        [HideInInspector]
        public static UIManager i;

        public DashboardManager BuildingsDashboard { get; private set; }
        public Scoreboard Scoreboard { get; private set; }
        private GameObject loanPanel;
        private GameObject actionsPanel;
        private GameObject approvePanel;
        private Coroutine validationCoroutine;

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

            this.Scoreboard = this.gameObject.GetComponentInChildren<Scoreboard>(true);

            ClientEventAggregator.Subscribe(this);

            loanPanel = transform.GetComponentsInChildren<Transform>(true).Single(x =>
                x.name.Equals("LoanPanel", StringComparison.InvariantCultureIgnoreCase)).gameObject;

            actionsPanel = transform.GetComponentsInChildren<Transform>(true).Single(x =>
                x.name.Equals("ActionBoard", StringComparison.InvariantCultureIgnoreCase)).gameObject;

            approvePanel = transform.GetComponentsInChildren<Transform>(true).Single(x =>
                x.name.Equals("ActionAppoveButtons", StringComparison.InvariantCultureIgnoreCase)).gameObject;

            BuildingsDashboard = transform.GetComponentsInChildren<Transform>(true)
                .Single(x => x.name.Equals("BuildingsDashboard", StringComparison.InvariantCultureIgnoreCase))
                .GetComponent<DashboardManager>();

            loanPanel.SetActive(false);
            actionsPanel.SetActive(false);
            approvePanel.SetActive(false);
            this.Scoreboard.gameObject.SetActive(false);
            BuildingsDashboard.gameObject.SetActive(false);
        }

        public void EnableScoreboardPanel()
        {
            this.Scoreboard.gameObject.SetActive(true);
        }

        public void DisableScoreboardPanel()
        {
            this.Scoreboard.gameObject.SetActive(false);
        }

        public void EnableApprovePanel()
        {
            approvePanel.SetActive(true);
        }

        public void DisableApprovePanel()
        {
            approvePanel.SetActive(false);
        }

        public void EnableLoanOptions(int playerIncome)
        {
            loanPanel.SetActive(true);
            var buttons = loanPanel.GetComponentsInChildren<LoanOptionsButton>(true);

            if (playerIncome == -8)
            {
                buttons.Single(x => x.LoanSize == LoanSize.BIG).gameObject.SetActive(false);
            }
            else if (playerIncome == -9)
            {
                buttons.Single(x => x.LoanSize == LoanSize.BIG).gameObject.SetActive(false);
                buttons.Single(x => x.LoanSize == LoanSize.MEDIUM).gameObject.SetActive(false);
            }
        }

        public void DisableLoanOptions()
        {
            loanPanel.SetActive(false);
        }

        public void EnableActionsBoard()
        {
            actionsPanel.SetActive(true);
        }

        public void DisableActionsBoard()
        {
            actionsPanel.SetActive(false);
        }

        public void EnableBuildingDashboard()
        {
            BuildingsDashboard.EnableDashboard();
        }

        public void DisableBuildingDashboard()
        {
            BuildingsDashboard.DisableDashboard();
        }

        public async void PlayEndTurnAnimation()
        {
            var label = Resources.Load<GameObject>("End Turn");
            var obj = Instantiate(label);
            obj.transform.localScale = new Vector3(1, 0);
            await EndTurnAnimation(obj);
        }

        public bool IsDashboardEmpty()
        {
            return BuildingsDashboard.IsEmpty();
        }

        public void ShowToast(string message)
        {
            if (validationCoroutine != null)
            {
                StopCoroutine(validationCoroutine);
            }

            // Container.GetMusicManager().Play(SoundConstants.Warning);
            validationCoroutine = this.StartCoroutine(this.ValidationErrorAnimation(message));
        }

        public void HandleUIClick(PlayerButtonType button)
        {
            switch (button)
            {
                case PlayerButtonType.ApproveAction:
                    GameManager.i.FinishAction();
                    break;
            }
        }

        private IEnumerator ValidationErrorAnimation(string message)
        {
            var text = GameObject.FindGameObjectWithTag("ValidationError").GetComponent<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            text.text = message;
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i <= 3; i++)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - 0.33f * i);
                yield return new WaitForSeconds(1f);
            }
        }

        private async Task EndTurnAnimation(GameObject obj)
        {
            do
            {
                if (obj.transform.localScale.y < 1)
                {
                    obj.transform.localScale = new Vector3(1, obj.transform.localScale.y + 0.01f);
                    await Task.Delay(Convert.ToInt16(Time.fixedDeltaTime * 100));
                }
                else
                {
                    obj.transform.localScale = new Vector3(1, 1);
                    await Task.Delay(2000);
                    Destroy(obj);
                    break;
                }
            } while (true);
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {

        }
    }
}
