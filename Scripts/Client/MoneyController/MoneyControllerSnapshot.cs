using Assets.Scripts.Client.BuildFeature;

namespace Assets.Scripts.Client.MoneyController
{
    public class MoneyControllerSnapshot : ISnapshot
    {
        public int Money { get; }
        public int Income { get; }
        public int CurrentIncomeKeyValue { get; }

        public MoneyControllerSnapshot(int money, int income, int currentIncomeKeyValue)
        {
            Money = money;
            Income = income;
            CurrentIncomeKeyValue = currentIncomeKeyValue;
        }
    }
}
