using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MenuScene
{
    public class MenuButton : MonoBehaviour
    {
        public MenuState state;

        void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(Click);
        }

        private void Click()
        {
            MenuStateManager.Instance.ChangeState(this.state);
        }
    }
}
