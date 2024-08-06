using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.Roads;
using Assets.Scripts.Common.Pathfind;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class City : MonoBehaviour
    {
        public CityNames Name;

        public List<Road> Roads()
        {
            return GameObject.Find("Board").GetComponent<MapGraph>().GetAllRoads().Where(r => r.CityA.Name == this.Name || r.CityB.Name == this.Name).ToList();
        }

        public List<Building> Buildings()
        {
            return transform
                .GetComponentsInChildren<Transform>()
                .Select(x => x.GetComponent<Building>())
                .Where(x => x != null)
                .ToList();
        }
    }
}
