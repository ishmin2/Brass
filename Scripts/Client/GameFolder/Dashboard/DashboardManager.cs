using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Client.GameFolder.Dashboard.UI;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.GameFolder.Dashboard
{
    public class DashboardManager : MonoBehaviour, ISnapshotable
    {
        [NonSerialized]
        public DashboardColumn[] columns;

        void Start()
        {
            this.columns = this.GetComponentsInChildren<DashboardColumn>();
        }

        public void EnableDashboard()
        {
            gameObject.SetActive(true);
        }

        public void DisableDashboard()
        {
            gameObject.SetActive(false);
        }

        public DashboardManager RestrictShipyardLevel1Button()
        {
            var button = this.columns
                .Where(x => x.Type == BuildingType.Shipyard)
                .SelectMany(x => x.Buttons)
                .Single(b => b.ButtonLevel == 1);

            button.SetRestrictState();

            return this;
        }

        public DashboardManager Disable2ndRoundButtons()
        {
            var buttons = this.columns
                .SelectMany(x => x.Buttons)
                .Where(b => b.ButtonLevel == 1).ToList();

            buttons.Add(this.columns.Single(x => x.Type == BuildingType.Shipyard).Buttons.Single(b => b.ButtonLevel == 2));
            foreach (var button in buttons)
            {
                button.SetLockState();
            }

            return this;
        }

        public DashboardManager ActivateAllButtons()
        {
            var buttons = this.columns.SelectMany(x => x.Buttons).ToArray();
            foreach (var dashboardButton in buttons)
            {
                dashboardButton.SetActiveState();
            }

            return this;
        }

        public DashboardManager DisableAllButtons()
        {
            var buttons = this.columns.SelectMany(x => x.Buttons).ToArray();
            foreach (var dashboardButton in buttons)
            {
                dashboardButton.SetIdleState();
            }

            return this;
        }

        public DashboardManager EnableButtonsByType(BuildingType type)
        {
            return this.EnableButtonsByType(new[] { type });
        }

        public DashboardManager EnableButtonsByType(BuildingType type, int startLevel)
        {
            return this.EnableButtonsByType(new[] { type }, startLevel);
        }

        public DashboardManager EnableButtonsByType(BuildingType[] types)
        {
            var buttons = this.columns.Where(c => types.Contains(c.Type)).SelectMany(b => b.Buttons).ToArray();
            foreach (var dashboardButton in buttons)
            {
                dashboardButton.SetActiveState();
            }

            return this;
        }

        public DashboardManager EnableButtonsByType(BuildingType[] types, int startLevel)
        {
            var buttons = this.columns.Where(c => types.Contains(c.Type)).SelectMany(b => b.Buttons).Where(b => b.ButtonLevel > startLevel).ToArray();
            foreach (var dashboardButton in buttons)
            {
                dashboardButton.SetActiveState();
            }

            return this;
        }

        public DashboardManager DistinctOnlyLowestLevelButtons()
        {
            foreach (var column in this.columns)
            {
                var buttons = column.Buttons.Where(b => !b.DashboardButtonCounter.IsEmpty()).ToArray();
                if (!buttons.Any())
                {
                    continue;
                }

                var lowestButtonLevelInColumn = buttons.Min(b => b.ButtonLevel);
                foreach (var dashboardButton in column.Buttons.OrderBy(b => b.ButtonLevel))
                {
                    if (dashboardButton.ButtonLevel > lowestButtonLevelInColumn)
                    {
                        dashboardButton.SetIdleState();
                    }
                }
            }

            return this;
        }

        public bool IsEmpty()
        {
            return this.columns.All(x => x.IsEmpty());
        }

        public ISnapshot Save()
        {
            var snap = new List<SnapshotTuple>();
            foreach (var column in this.columns)
            {
                foreach (var columnButton in column.Buttons)
                {
                    snap.Add(new SnapshotTuple { Type = column.Type, Count = columnButton.DashboardButtonCounter.Count, Level = columnButton.ButtonLevel });
                }

            }

            return new DashboardManagerSnapshot(snap);
        }

        public void Restore(ISnapshot snapshot)
        {
            if (!(snapshot is DashboardManagerSnapshot))
            {
                throw new Exception("Unknown snapshot class " + snapshot);
            }

            var snapshotTyped = snapshot as DashboardManagerSnapshot;
            foreach (var column in this.columns)
            {
                foreach (var columnButton in column.Buttons)
                {
                    var count = snapshotTyped.Buttons.Single(x =>
                        x.Level == columnButton.ButtonLevel && x.Type == column.Type).Count;

                    columnButton.DashboardButtonCounter.SetCount(count);
                }
            }
        }
    }
}
