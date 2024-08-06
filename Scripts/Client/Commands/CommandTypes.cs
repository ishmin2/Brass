using System;

namespace Assets.Scripts.Client.Commands
{
    [Serializable]
    public enum CommandTypes
    {
        FailedToGetRequestType,
        MarketBuyResourceRequestModel,
        RoadBuiltRequestModel,
        ChangeMoneySyncVarRequestModel,
        BuildingSellResourceRequestModel,
        BuildingConstructedRequestModel,
        CardPlayedRequestModel,
        ActivateBuildingRequestModel,
    }
}
