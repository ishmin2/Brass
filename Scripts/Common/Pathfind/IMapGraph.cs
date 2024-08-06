using System.Collections.Generic;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.Roads;

namespace Assets.Scripts.Common.Pathfind
{
    public interface IMapGraph
    {
        City GetCityByName(CityNames cityName);

        IEnumerable<Building> GetBuildingsByType(BuildingType type);

        IEnumerable<City> GetCities();

        IEnumerable<Road> GetAllRoads();

        IEnumerable<Building> GetBuildings();

        IEnumerable<Road> GetAllRoadsByPlayer(PlayerColors playerColor);

        IEnumerable<Road> GetAllRoadsByPlayerInNetwork(PlayerColors playerColor);

        IEnumerable<Road> GetAllRoadsWithAccessToCoal();

        IEnumerable<City> GetAllCitiesInTradeNetwork(PlayerColors playerColor);

        IEnumerable<Building> GetBuildingsByCity(CityNames cityName);
    }
}
