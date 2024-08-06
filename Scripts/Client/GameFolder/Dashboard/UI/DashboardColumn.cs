using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.GameFolder.Dashboard.UI
{
    public class DashboardColumn : MonoBehaviour
    {
        public BuildingType Type;

        public List<DashboardButton> Buttons { get; private set; }

        void Awake()
        {
            Buttons = this.GetComponentsInChildren<DashboardButton>().ToList();
        }

        public void Activate()
        {
            var correctButtons = Buttons.Where(x => !x.DashboardButtonCounter.IsEmpty()).ToList();
            if (correctButtons.Any())
            {
                var lowest = correctButtons.Aggregate((currMin, b) => currMin.ButtonLevel > b.ButtonLevel ? b : currMin);
                lowest.GetComponent<Image>().enabled = true;
                lowest.GetComponent<Button>().enabled = true;
            }
        }

        public void Disable()
        {
            var correctButtons = Buttons.Where(x => !x.DashboardButtonCounter.IsEmpty());
            foreach (var button in correctButtons)
            {
                button.GetComponent<Image>().enabled = false;
                button.GetComponent<Button>().enabled = false;
            }
        }

        public bool IsEmpty()
        {
            return Buttons.All(x => x.DashboardButtonCounter.IsEmpty());
        }
    }
}
