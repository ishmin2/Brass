using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Common.Pathfind
{
    public static class Pathfinder
    {
        public static List<City> SearchAvailableCities(City start, MapGraph graph)
        {
            Dictionary<City, bool> used = new Dictionary<City, bool>();
            foreach (var city in graph.GetCities())
            {
                used.Add(city, false);
            }
            used[start] = true;

            Queue<City> queue = new Queue<City>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var city = queue.Dequeue();
                var roads = city.Roads();

                foreach (var road in roads)
                {
                    if (used[road.CityA] && used[road.CityB])
                    {
                        continue;
                    }

                    if (road.CityB == city)
                    {
                        if (!used[road.CityA] && road.PlayerColor != PlayerColors.None)
                        {
                            queue.Enqueue(road.CityA);
                            used[road.CityA] = true;
                        }
                    }
                    else if (!used[road.CityB] && road.PlayerColor != PlayerColors.None)
                    {
                        queue.Enqueue(road.CityB);
                        used[road.CityB] = true;
                    }
                }
            }

            return used.Where(x => x.Value).Select(x => x.Key).ToList();
        }

        public static KeyValuePair<List<City>, int> SearchClosestCoalCities(City start, MapGraph graph)
        {
            Dictionary<City, bool> used = new Dictionary<City, bool>();
            foreach (var city in graph.GetCities())
            {
                used.Add(city, false);
            }
            used[start] = true;

            Queue<City> queue = new Queue<City>();
            queue.Enqueue(start);

            var step = 1;
            while (queue.Any())
            {
                if (used.Where(x => x.Value)
                    .Any(x => x.Key.Buildings().Any(b => b.CoalToSellRemind > 0)))
                {
                    break;
                }

                var city = queue.Dequeue();
                var roads = city.Roads();

                foreach (var road in roads)
                {
                    if (used[road.CityA] && used[road.CityB])
                    {
                        continue;
                    }

                    if (road.CityB == city)
                    {
                        if (!used[road.CityA] && road.PlayerColor != 0)
                        {
                            queue.Enqueue(road.CityA);
                            used[road.CityA] = true;
                        }
                    }
                    else if (!used[road.CityB] && road.PlayerColor != 0)
                    {
                        queue.Enqueue(road.CityB);
                        used[road.CityB] = true;
                    }
                }

                step++;
            }

            var result = new KeyValuePair<List<City>, int>(
                used.Where(x => x.Value)
                        .Where(x => x.Key.Buildings()
                        .Any(b => b.CoalToSellRemind > 0))
                        .Select(x => x.Key)
                        .Distinct()
                        .ToList(),
                    step);

            return result;
        }

        public static List<City> SearchAvailableCitiesInTradeNetwork(City start, PlayerColors playerColor, MapGraph graph)
        {
            Dictionary<City, bool> used = new Dictionary<City, bool>();
            foreach (var city in graph.GetCities())
            {
                used.Add(city, false);
            }
            used[start] = true;

            Queue<City> queue = new Queue<City>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var city = queue.Dequeue();
                var roads = city.Roads();

                foreach (var road in roads)
                {
                    if (!used[road.CityB] && road.PlayerColor == playerColor)
                    {
                        queue.Enqueue(road.CityB);
                        used[road.CityB] = true;
                    }
                }
            }

            return used.Where(x => x.Value).Select(x => x.Key).ToList();
        }

        public static bool CanSellToWorldTrade(City city, MapGraph graph)
        {
            var connectedCities = SearchAvailableCities(city, graph);
            var worldTradeCities = connectedCities.SelectMany(c => c.Buildings()
                .Where(b => b.CanWorldTrade || b.BuildingType == BuildingType.WorldTrade)).ToArray();

            if (worldTradeCities.Any())
            {
                return true;
            }

            return false;
        }
    }
}
