using Assets.Scripts.Client.StaticObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.MainScene.UI
{
    public class PlayerButtons : MonoBehaviour, IPointerClickHandler
    {
        public PlayerButtonType type;

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (type)
            {
                case PlayerButtonType.ApproveAction:
                    GameManager.i.currentPlayerAction.FinishAction();
                    break;
                case PlayerButtonType.DeclineAction:
                    break;
            }
        }
    }
}