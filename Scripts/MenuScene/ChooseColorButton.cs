using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MenuScene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChooseColorButton : MonoBehaviour
    {
        public PlayerColors Color;

        void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(Click);
        }

        public void Click()
        {
            MenuStateManager.Instance.ChoosePlayerColor(this);
        }
    }
}
