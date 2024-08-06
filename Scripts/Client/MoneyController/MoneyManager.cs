using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.UI;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.MoneyController
{
    public class MoneyManager : MonoBehaviour, ISnapshotable
    {
        public int CurrentIncomeKeyValue { get; set; }
        private readonly Dictionary<int, int> MoneyIncomePairs = new Dictionary<int, int>();
        private Func<PlayerPanel> playerPanel { get; } = () => Container.Instance().GetPlayerPanel();

        void Awake()
        {
            this.InitializeMoneyIncomePairs();
            this.CurrentIncomeKeyValue = 10;
        }

        public void GetLoan(LoanSize loan)
        {
            var addMoney = this.LoanSizeToMoney(loan);
            this.ChangeMoney(addMoney);

            // TODO: fix it New method get loan
            var delta = this.LoanSizeToIncome(loan);
            this.ChangeIncome(delta);

            ClientEventAggregator.Publish(new ChangeMoneyRequestModel
            {
                // PlayerColor = Container.GetLocalPlayer().PlayerColor,
                Money = addMoney,
                Income = delta,
                IncomeTrackPosition = this.CurrentIncomeKeyValue,
                IsLoan = true
            });
        }

        public void ChangeMoney(int count)
        {
            this.playerPanel().SetLocalMoney(count);
        }

        public void ChangeIncome(int count)
        {
            CurrentIncomeKeyValue += count;
            if (CurrentIncomeKeyValue > 99)
            {
                CurrentIncomeKeyValue = 99;
            }

            var newIncome = this.MoneyIncomePairs.Single(pair => pair.Key == CurrentIncomeKeyValue).Value;
            this.playerPanel().SetLocalIncome(newIncome);
        }

        public void AddTurnIncome()
        {
            var money = Container.Instance().GetPlayerPanel().LocalMoney + Container.Instance().GetPlayerPanel().LocalIncome;
            // ClientEventAggregator.Publish(new ChangeMoneyRequestModel { PlayerColor = Container.GetLocalPlayer().PlayerColor, Money = money, IsLoan = true });
        }

        private int LoanSizeToMoney(LoanSize size)
        {
            switch (size)
            {
                case LoanSize.SMALL:
                    return 10;
                case LoanSize.MEDIUM:
                    return 20;
                case LoanSize.BIG:
                    return 30;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        private int LoanSizeToIncome(LoanSize size)
        {
            switch (size)
            {
                case LoanSize.SMALL:
                    return -1;
                case LoanSize.MEDIUM:
                    return -2;
                case LoanSize.BIG:
                    return -3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        private void InitializeMoneyIncomePairs()
        {
            int income = -10;
            int div = 1;
            for (int i = 0; i < 100; i++)
            {
                if (i < 10)
                {
                }
                else if (i < 30)
                {
                    div = 2;
                }
                else if (i < 60)
                {
                    div = 3;
                }
                else if (i < 96)
                {
                    div = 4;
                }
                else if (i < 100)
                {
                    div = 3;
                }

                this.MoneyIncomePairs.Add(i, income);
                if (i % div == 0)
                {
                    income++;
                }
            }
        }

        public ISnapshot Save()
        {
            return new MoneyControllerSnapshot(0, 0, 0);
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is MoneyControllerSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = (MoneyControllerSnapshot)snapshot;
            CurrentIncomeKeyValue = snapshotTyped.CurrentIncomeKeyValue;
        }
    }
}