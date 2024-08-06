using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.UI.Score
{
    public class ScoreController : MonoBehaviour
    {
        public ScoreTrack Purple;
        public ScoreTrack Red;
        public ScoreTrack Yellow;
        public ScoreTrack White;

        public void SetColors(PlayerColors[] colors)
        {
            this.Purple.gameObject.SetActive(false);
            this.Red.gameObject.SetActive(false);
            this.Yellow.gameObject.SetActive(false);
            this.White.gameObject.SetActive(false);

            foreach (var playerColor in colors)
            {
                switch (playerColor)
                {
                    case PlayerColors.Red:
                        this.Red.gameObject.SetActive(true);
                        break;
                    case PlayerColors.Purple:
                        this.Purple.gameObject.SetActive(true);
                        break;
                    case PlayerColors.White:
                        this.White.gameObject.SetActive(true);
                        break;
                    case PlayerColors.Yellow:
                        this.Yellow.gameObject.SetActive(true);
                        break;
                }
            }
        }

        public void SetValue(PlayerColors color, int value)
        {
            var strValue = value.ToString();
            switch (color)
            {
                case PlayerColors.Red:
                    this.Red.ScoreText.text = strValue;
                    break;
                case PlayerColors.Purple:
                    this.Purple.ScoreText.text = strValue;
                    break;
                case PlayerColors.White:
                    this.White.ScoreText.text = strValue;
                    break;
                case PlayerColors.Yellow:
                    this.Yellow.ScoreText.text = strValue;
                    break;
            }
        }

        public void ChangePositionsByDesc()
        {
            var arr = new[] { this.Purple, this.Red, this.Yellow, this.White }.OrderByDescending(el => int.Parse(el.ScoreText.text)).ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
