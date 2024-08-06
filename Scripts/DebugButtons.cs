using System.Linq;
using Assets.Scripts.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DebugButtons : MonoBehaviour
    {
        public void OnClick()
        {
            if (Container.Instance().GetGameState().GameRound == 1)
            {
                Container.Instance().GetGameState().GameRound = 2;
                var obj = GameObject.FindGameObjectWithTag("RoundMarker");
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round1")).gameObject
                    .GetComponent<Image>().enabled = false;
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round2")).gameObject
                    .GetComponent<Image>().enabled = true;
            }
            else
            {
                Container.Instance().GetGameState().GameRound = 1;
                var obj = GameObject.FindGameObjectWithTag("RoundMarker");

                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round1")).gameObject
                    .GetComponent<Image>().enabled = true;
                obj.GetComponentsInChildren<Transform>().Single(x => x.name.Equals("Round2")).gameObject
                    .GetComponent<Image>().enabled = false;
            }
        }
    }
}
