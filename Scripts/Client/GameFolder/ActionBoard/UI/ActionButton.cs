using Assets.Scripts.Client.StaticObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Client.GameFolder.ActionBoard.UI
{
    public class ActionButton : MonoBehaviour, IDropHandler
    {
        public ActionType Action;

        public void OnDrop(PointerEventData eventData)
        {
            GameManager.i.CurrentPlayerCard = eventData.pointerDrag.GetComponent<PlayerCard>();
            GameManager.i.StartPlayerAction(this.Action);
        }
    }
}
