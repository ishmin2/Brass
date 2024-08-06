using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature.Roads;
using Assets.Scripts.Client.BuildFeature.Roads.States;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Pathfind;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature
{
    public class BuildingManager : MonoBehaviour
    {
        [HideInInspector]
        public static BuildingManager i;

        void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else if (i == this)
            {
                Destroy(gameObject);
            }
        }

        public void ActivateRoads(IEnumerable<Road> roads)
        {
            foreach (var road in roads)
            {
                road.SetState(new RoadActiveState(road));
            }
        }

        public void DeactivateRoads()
        {
            var roads = MapGraph.instance.GetAllRoads().Where(r => r.PlayerColor == PlayerColors.None).ToList();
            foreach (var road in roads)
            {
                road.SetState(new RoadIdleState(road));
            }
        }
        
        public void DeactivateRoads(List<Road> roads)
        {
            foreach (var road in roads)
            {
                road.SetState(new RoadIdleState(road));
            }
        }


        public List<Building> ActivateBuildingByFactoryType(BuildingType type)
        {
            var buildings = MapGraph.instance.GetBuildingsByType(type).ToList();
            this.ActivateBuildings(buildings);
            return buildings;
        }

        public void ActivateBuildingByFactoryType(BuildingType type, PlayerColors playerColor)
        {
            var buildings = MapGraph.instance.GetBuildingsByType(type).Where(b => b.PlayerColor == playerColor && b.isActivated == false).ToList();
            ActivateBuildings(buildings);
        }

        public void ActivateBuildings(IEnumerable<Building> buildings)
        {
            foreach (var building in buildings)
            {
                building.SetSellState();
            }
        }

        public void DeactivateBuildingByFactoryType(BuildingType type)
        {
            var buildings = MapGraph.instance.GetBuildingsByType(type);

            foreach (var building in buildings)
            {
                building.SetIdleState();
            }
        }
        
        public bool isUserHasTradeNetwork(PlayerColors playerColor)
        {
            var cities = MapGraph.instance.GetCities().ToArray();
            var buildings = cities.Any(c => c.Buildings().Any(b => b.PlayerColor == playerColor));
            var roads = cities.Any(c => c.Roads().Any(r => r.PlayerColor == playerColor));

            return buildings || roads;
        }
    }
}
