using Assets.Scripts.Client.Events;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.MoneyController
{
    public class LoanOptionsButton : MonoBehaviour
    {
        public LoanSize LoanSize;

        void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(this.OnClick);
        }

        void OnClick()
        {
            ClientEventAggregator.Publish(new LoanTakenActionEvent(this.LoanSize));
        }
    }
}
