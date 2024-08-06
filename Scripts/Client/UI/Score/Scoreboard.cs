using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Client.UI.Score
{
    public class Scoreboard : MonoBehaviour
    {
        public ScoreController Factory;
        public ScoreController Road;
        public ScoreController Money;

        public void GetScore()
        {
            this.Factory.gameObject.SetActive(true);
            this.Road.gameObject.SetActive(true);
            this.Money.gameObject.SetActive(true);

            var panels = Container.GetPlayerPanels();
            var colors = panels.Select(pp => pp.Color).ToArray();

            this.Factory.SetColors(colors);
            this.Road.SetColors(colors);
            this.Money.SetColors(colors);

            if (Container.Instance().GetGameState().GameRound == 1)
            {
                this.Money.gameObject.SetActive(false);
            }

            foreach (var playerPanel in panels)
            {
                var victoryPointsForFactories = Container.GetMapGraph().GetCities()
                    .SelectMany(x => x.Buildings().Where(b => b.PlayerColor == playerPanel.Color && b.isActivated))
                    .Sum(b => b.VictoryPoints);
                this.Factory.SetValue(playerPanel.Color, victoryPointsForFactories);

                var victoryPointsForRoads = Container.GetMapGraph().GetAllRoadsByPlayer(playerPanel.Color).Sum(r =>
                    r.CityA.Buildings().Count(b => b.isActivated) + r.CityB.Buildings().Count(b => b.isActivated));
                this.Road.SetValue(playerPanel.Color, victoryPointsForRoads);

                if (Container.Instance().GetGameState().GameRound == 2)
                {
                    this.Money.SetValue(playerPanel.Color, playerPanel.Money / 10);
                }
            }
        }
    }
}
