using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.Roads;
using UnityEngine;

namespace Assets.Scripts.Common.Pathfind
{
    public class MapGraph : MonoBehaviour, IMapGraph
    {
        [HideInInspector]
        public static MapGraph instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
        }

        public IEnumerable<Building> GetBuildingsByType(BuildingType type)
        {
            return this.GetBuildings().Where(s => s.BuildingType == type).ToList();
        }

        public City GetCityByName(CityNames cityName)
        {
            return this.GetCities().Single(city => city.Name.Equals(cityName));
        }

        public IEnumerable<City> GetCities()
        {
            return gameObject
                .GetComponentsInChildren<Transform>()
                .Single(x => x.name.Equals("Cities"))
                .GetComponentsInChildren<City>()
                .ToList();
        }

        public IEnumerable<Road> GetAllRoads()
        {
            return gameObject
                .GetComponentsInChildren<Transform>()
                .Single(x => x.name.Equals("Roads"))
                .GetComponentsInChildren<Road>()
                .ToList();
        }

        public IEnumerable<Building> GetBuildings()
        {
            return this.GetCities().SelectMany(c => c.Buildings()).ToList();
        }

        public IEnumerable<Road> GetAllRoadsByPlayer(PlayerColors playerColor)
        {
            return this.GetAllRoads().Where(x => x.PlayerColor == playerColor);
        }

        public IEnumerable<Road> GetAllRoadsByPlayerInNetwork(PlayerColors playerColor)
        {
            var roadsFromPlayerCities = this.GetAllCitiesInTradeNetwork(playerColor).SelectMany(c => c.Roads());
            var roadsByPlayer = this.GetAllRoadsByPlayer(playerColor);
            return roadsFromPlayerCities.Union(roadsByPlayer).Distinct();
        }

        public IEnumerable<Road> GetAllRoadsWithAccessToCoal()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<City> GetAllCitiesInTradeNetwork(PlayerColors playerColor)
        {
            var cities = this.GetCities().Where(x => x.Buildings().Any(b => b.PlayerColor == playerColor));
            var playerRoads = this.GetAllRoads().Where(r => r.PlayerColor == playerColor);
            var citiesByRoads = playerRoads.SelectMany(r => new[] { r.CityA, r.CityB });
            return cities.Union(citiesByRoads).Distinct();
        }

        public IEnumerable<Building> GetBuildingsByCity(CityNames cityName)
        {
            return this.GetCities().Single(c => c.Name == cityName).Buildings();
        }
    }
}
