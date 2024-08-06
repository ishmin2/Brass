using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    public class ChangeMoneyRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.ChangeMoneySyncVarRequestModel;

        public int Money;

        public int Income;

        public int IncomeTrackPosition;
        
        public bool IsLoan;

        public PlayerColors PlayerColor;
    }
}
