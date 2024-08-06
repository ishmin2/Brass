using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.Commands.Models
{
    [Serializable]
    public class MarketBuyResourceRequestModel : IServerAction
    {
        [SerializeField]
        private CommandTypes commandType = CommandTypes.MarketBuyResourceRequestModel;

        public MarketType MarketType;

        public int Cost;
        
        public MarketBuyResourceRequestModel(MarketType marketType, int cost)
        {
            this.MarketType = marketType;
            this.Cost = cost;
        }
    }
}
