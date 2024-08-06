using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.BuildFeature.Roads;
using Assets.Scripts.Client.BuildFeature.Roads.States;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.Interactions.CoalMarket;
using Assets.Scripts.Client.Interactions.CottonMarket.Events;
using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Common.Pathfind;
using Assets.Scripts.Common.PlayerActions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.MainScene.Data;
using UnityEngine;

namespace Assets.Scripts.Common.PlayerActions
{
    public class RoadAction : IPlayerAction
    {
        public Dictionary<ISnapshotable, ISnapshot> Snapshotables { get; set; }

        private readonly Player player;

        private int roadBuiltCount;
        private bool coalBought;
        private bool canFinishAction;
        private readonly CancellationTokenSource cts;
        private readonly int roadCost = GameManager.i.GameState.GameRound == 1 ? 3 : 5;

        public RoadAction(Player player)
        {
            this.player = player;
            this.cts = new CancellationTokenSource();
            Snapshotables = new Dictionary<ISnapshotable, ISnapshot>();
            ClientEventAggregator.Subscribe(this);
        }

        public bool CanStartAction(ref string reason)
        {
            var roads = this.GetAvailableRoads(GameManager.i.GameState.GameRound).ToList();
            var hasError = false;
            if (this.IsRoadLimitReached())
            {
                reason = "Roads limit reached";
                hasError = true;
            }
            else if (!roads.Any())
            {
                reason = "No available roads to build";
                hasError = true;
            }
            else if (this.player.Money < roadCost)
            {
                reason = "Not enough money";
                hasError = true;
            }

            return !hasError;
        }

        public void StartAction()
        {
            var roads = this.GetAvailableRoads(GameManager.i.GameState.GameRound).ToList();
            if (this.roadBuiltCount == 2 || this.IsRoadLimitReached() || !roads.Any())
            {
                MusicManager.i.Play(SoundConstants.JobDone);
                return;
            }

            if (this.roadBuiltCount == 0)
            {
                this.SnapshotBeforeAction(MapGraph.instance.GetAllRoads().ToList());
            }

            BuildingManager.i.ActivateRoads(roads);
        }

        public bool CanFinishAction(ref string reason)
        {
            if (this.player.Money > 0 && this.canFinishAction)
            {
                return true;
            }

            reason = this.canFinishAction ? "Not enough money" : "Road not built";
            return false;
        }

        public void FinishAction()
        {
            string reason = string.Empty;
            if (this.CanFinishAction(ref reason))
            {
                ClientEventAggregator.Unsubscribe(this);
                BuildingManager.i.DeactivateRoads();
                CoalMarket.instance.SetState(new CoalMarketIdleState(CoalMarket.instance));
                GameManager.i.FinishAction();
            }
            else
            {
                UIManager.i.ShowToast(reason);
            }
        }

        public void CancelAction()
        {
            try
            {
                this.cts.Cancel();
                this.cts.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log("[Error] " + e);
            }
        }

        public void HandleEvent<T>(T eventType) where T : IActionEvent
        {
            if (eventType is RoadChosenActionEvent roadChosenEvent)
            {
                this.OnRoadChosenEvent(roadChosenEvent);
            }

            if (eventType is MarketBuyResourceRequestModel marketBuyResource && marketBuyResource.MarketType == MarketType.CoalMarket)
            {
                this.player.Money += -marketBuyResource.Cost;
                CoalMarket.instance.SetState(new CoalMarketIdleState(CoalMarket.instance));
                this.coalBought = true;
            }

            if (eventType is BuildingSellResourceRequestModel)
            {
                this.coalBought = true;
            }
        }

        private async void OnRoadChosenEvent(RoadChosenActionEvent roadChosenActionEvent)
        {
            BuildingManager.i.DeactivateRoads();
            this.roadBuiltCount++;

            switch (GameManager.i.GameState.GameRound)
            {
                case 1:
                    {
                        var cost = -3;
                        this.player.Money += cost;
                        roadChosenActionEvent.road.SetState(new RoadBuiltState(roadChosenActionEvent.road, this.player.Color));

                        MusicManager.i.Play(SoundConstants.BoatBuilt);
                        // MusicManager.i.Play(SoundConstants.JobDone);
                        this.canFinishAction = true;
                        break;
                    }
                case 2:
                    {
                        this.canFinishAction = false;
                        roadChosenActionEvent.road.SetState(new RoadGhostState(roadChosenActionEvent.road));
                        MusicManager.i.Play(SoundConstants.TrainBuilt);
                        this.EnableCoalBuyObject(roadChosenActionEvent.road);

                        var awaitCoal = Task.Run(this.WaitCoal, this.cts.Token);
                        await awaitCoal;
                        if (this.cts.IsCancellationRequested)
                            return;

                        this.canFinishAction = true;
                        BuildingManager.i.DeactivateBuildingByFactoryType(BuildingType.CoalMine);

                        var cost = -5 * this.roadBuiltCount;
                        this.player.Money += cost;
                        roadChosenActionEvent.road.SetState(new RoadBuiltState(roadChosenActionEvent.road, this.player.Color));

                        this.coalBought = false;
                        this.StartAction();
                        break;
                    }
            }
        }

        private void EnableCoalBuyObject(Road road)
        {
            var buildings = this.GetClosestCitiesByRoad(road).ToArray();
            var coalMines = buildings.Where(b => b.BuildingType == BuildingType.CoalMine).ToArray();
            if (coalMines.Any())
            {
                BuildingManager.i.ActivateBuildings(coalMines);
            }
            else
            {
                CoalMarket.instance.SetState(new CoalMarketTradeState(CoalMarket.instance));
            }
        }

        private IEnumerable<Road> GetAvailableRoads(int gameRound)
        {
            var tradeNetworkExist = BuildingManager.i.isUserHasTradeNetwork(this.player.Color);
            return !tradeNetworkExist
                ? this.GetAllRoadsByRound(gameRound).Distinct()
                : this.GetRoadsInNetworkByRound(this.player.Color).Distinct();
        }

        private bool IsRoadLimitReached()
        {
            return MapGraph.instance.GetAllRoads().Count(x => x.PlayerColor == this.player.Color) == 14;
        }

        private void SnapshotBeforeAction(List<Road> snapshotRoads)
        {
            //Snapshotables.Add(this.moneyManager, this.moneyManager.Save());
            Snapshotables.Add(CoalMarket.instance, CoalMarket.instance.Save());
            foreach (var road in snapshotRoads)
            {
                Snapshotables.Add(road, road.Save());
            }

            foreach (var coalMine in MapGraph.instance.GetBuildingsByType(BuildingType.CoalMine))
            {
                Snapshotables.Add(coalMine, coalMine.Save());
            }
        }

        private IEnumerable<Road> GetAllRoadsByRound(int gameRound)
        {
            var allRoads = MapGraph.instance.GetAllRoads();
            if (gameRound == 1)
            {
                return allRoads.Where(r => r.isRiverAvailable && r.PlayerColor == PlayerColors.None).ToList();
            }

            List<Road> roadsByRound = new List<Road>();
            foreach (var road in allRoads)
            {
                if (this.GetCoalCitiesByRoad(road).Any())
                {
                    roadsByRound.Add(road);
                }
            }

            return roadsByRound;
        }

        private IEnumerable<Road> GetRoadsInNetworkByRound(PlayerColors playerColor)
        {
            var resultRoads = new List<Road>();

            var roadsInNetwork = MapGraph.instance.GetAllRoadsByPlayerInNetwork(playerColor).Where(x => x.PlayerColor == PlayerColors.None).ToList();
            if (GameManager.i.GameState.GameRound == 1)
            {
                resultRoads = roadsInNetwork.Where(r => r.isRiverAvailable).ToList();
            }

            if (GameManager.i.GameState.GameRound == 2)
            {
                foreach (var road in roadsInNetwork)
                {
                    if (this.GetCoalCitiesByRoad(road).Any())
                    {
                        resultRoads.Add(road);
                    }
                }
            }

            return resultRoads.Where(r => r.PlayerColor == 0);
        }

        private IEnumerable<Building> GetCoalCitiesByRoad(Road road)
        {
            var allCitiesAccessByRoad = Pathfinder.SearchAvailableCities(road.CityA, MapGraph.instance).Union(Pathfinder.SearchAvailableCities(road.CityB, MapGraph.instance)).ToList();
            return allCitiesAccessByRoad.SelectMany(c => c.Buildings().Where(b => b.CoalToSellRemind > 0 || b.CanWorldTrade));
        }

        private IEnumerable<Building> GetClosestCitiesByRoad(Road road)
        {
            var allCitiesAccessByRoadA = Pathfinder.SearchClosestCoalCities(road.CityA, MapGraph.instance);
            var allCitiesAccessByRoadB = Pathfinder.SearchClosestCoalCities(road.CityB, MapGraph.instance);
            List<City> totalCities;

            if (allCitiesAccessByRoadA.Value < allCitiesAccessByRoadB.Value)
            {
                totalCities = allCitiesAccessByRoadA.Key;
            }
            else if (allCitiesAccessByRoadA.Value > allCitiesAccessByRoadB.Value)
            {
                totalCities = allCitiesAccessByRoadB.Key;
            }
            else
            {
                totalCities = allCitiesAccessByRoadA.Key.Union(allCitiesAccessByRoadB.Key).ToList();
            }

            return totalCities.SelectMany(c => c.Buildings().Where(b => b.CoalToSellRemind > 0 || b.CanWorldTrade));
        }

        private async Task WaitCoal()
        {
            while (!this.coalBought && !this.cts.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
        }
    }
}
