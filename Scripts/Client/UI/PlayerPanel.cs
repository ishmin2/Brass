using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Client.UI
{
    public class PlayerPanel //: NetworkBehaviour
    {
        public PlayerColors Color;

        public int CardCount;

        public int Money;

        public int Income;

        public int MoneySpent;

        public int IncomeTrackPosition;

        public int LocalMoney { get; private set; }
        public int LocalIncome { get; private set; }

        public Text CardCountText;

        public Text MoneyText;

        public Text IncomeText;

        public Text MoneySpentText;

        public Text LocalMoneyText;

        public Text LocalIncomeText;

        public Text LocalMoneySpentText;

        [HideInInspector]
        public string pathToSprite;

        private int TurnMoneyChange;

        private int TurnIncomeChange;

        void Start()
        {
            //transform.SetParent(GameObject.FindGameObjectWithTag("PlayerPanel").transform);
            this.MoneyText.text = "30";
            this.IncomeText.text = "0";

            this.Money = 30;
            this.Income = 0;

            this.LocalMoney = this.Money;
            this.LocalIncome = this.Income;
        }

        public void SetLocalMoney(int moneyDelta)
        {
            this.MoneyText.GetComponent<Text>().enabled = false;
            this.LocalMoneyText.GetComponent<Text>().enabled = true;

            this.TurnMoneyChange += moneyDelta;
            this.LocalMoneyText.text = TurnMoneyChange > 0 ? $"{this.Money}(+{TurnMoneyChange})" : $"{this.Money}({TurnMoneyChange})";
            this.LocalMoney = this.Money + TurnMoneyChange;
            
        }

        public void SetLocalIncome(int incomeDelta)
        {
            this.IncomeText.GetComponent<Text>().enabled = false;
            this.LocalIncomeText.GetComponent<Text>().enabled = true;

            this.TurnIncomeChange = incomeDelta;
            this.LocalIncomeText.text = TurnIncomeChange > 0 ? $"{this.Income}(+{TurnIncomeChange})" : $"{this.Income}({TurnIncomeChange})";
            this.LocalIncome = this.Income + TurnIncomeChange;
        }

        public void DisableLocalTexts()
        {
            this.LocalMoneyText.GetComponent<Text>().enabled = false;
            this.LocalIncomeText.GetComponent<Text>().enabled = false;
            this.LocalMoneySpentText.GetComponent<Text>().enabled = false;

            this.MoneyText.GetComponent<Text>().enabled = true;
            this.IncomeText.GetComponent<Text>().enabled = true;
            this.MoneySpentText.GetComponent<Text>().enabled = true;

            this.TurnMoneyChange = 0;
            this.TurnIncomeChange = 0;
            this.LocalMoney = this.Money;
        }
    }
}
