using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Client.GameFolder.Dashboard.UI
{
    public class DashboardButtonCounter : MonoBehaviour
    {
        public int TotalCount;
        public int Count;
        private Image image;

        void Awake()
        {
            this.image = gameObject.GetComponent<Image>();
            this.Count = this.TotalCount;
        }

        public void RemoveOne()
        {
            if (!this.IsEmpty())
            {
                this.Count--;
                this.image.fillAmount -= 1 / (float)this.TotalCount;
            }
        }

        public bool IsEmpty()
        {
            return this.Count == 0;
        }

        public void SetCount(int count)
        {
            this.Count = count;
            this.image.fillAmount = (1 / (float) this.TotalCount) * count;
        }
    }
}
