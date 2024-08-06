using System.Threading;
using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Constants;
using Assets.Scripts.MainScene.Data;
using Assets.Scripts.MainScene.GameState;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.MenuScene
{
    public class MenuStateManager : MonoBehaviour
    {
        [HideInInspector]
        public static MenuStateManager Instance;

        public GameObject MainMenu;
        public GameObject ChoosePlayer;
        public InputField PlayerName;
        public Slider AiCount;

        private ChooseColorButton chosenColorButton;

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            SceneManager.LoadScene("MainScene");
        }

        public void ChoosePlayerColor(ChooseColorButton button)
        {
            if (this.chosenColorButton != null)
            {
                var color = this.chosenColorButton.GetComponent<Image>().color;
                this.chosenColorButton.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
            }
            this.chosenColorButton = button;
            var colorNew = this.chosenColorButton.GetComponent<Image>().color;
            this.chosenColorButton.GetComponent<Image>().color = new Color(colorNew.r, colorNew.g, colorNew.b, 0.5f);
        }

        public void ChangeState(MenuState state)
        {
            switch (state)
            {
                case MenuState.MainMenu:
                    this.MainMenu.SetActive(true);
                    this.ChoosePlayer.SetActive(false);
                    break;
                case MenuState.ChoosePlayer:
                    this.MainMenu.SetActive(false);
                    this.ChoosePlayer.SetActive(true);
                    break;
                case MenuState.Settings:
                    break;
                case MenuState.StartGame:
                    AddPlayers();
                    SceneManager.LoadScene("MainScene");
                    break;
                case MenuState.Exit:
                    Application.Quit();
                    break;
                default:
                    return;
            }
        }

        private void AddPlayers()
        {
            PlayersManager.i.AddPlayer(new Player { Name = PlayerName.text, Color = chosenColorButton.Color, PlayerType = PlayerType.Human });
            GameManager.i.PlayerColor = chosenColorButton.Color;

            for (int i = 0; i < AiCount.value + 1; i++)
            {
                var freeColor = PlayersManager.i.GetFreePlayerColor()[0];
                PlayersManager.i.AddPlayer(new Player { Name = $"AI {i}", Color = freeColor, PlayerType = PlayerType.AI });
            }
        }
    }
}
