using System;
using Assets.Scripts.Client.Events;
using Assets.Scripts.Client.StaticObjects;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class PlayerCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public CityNames City;
        public BuildingType Type;

        private GameObject cardBigPicture;

        void Start()
        {
            this.cardBigPicture = new GameObject("CardBigPicture");
            this.cardBigPicture.AddComponent<Image>();
            this.cardBigPicture.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            this.cardBigPicture.transform.localScale = new Vector3(1.8f, 2);
            this.cardBigPicture.transform.SetParent(transform);
            this.cardBigPicture.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GameManager.i.DragCard(this);
            this.cardBigPicture.SetActive(false);
            this.GetComponent<Image>().color = Color.white;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameManager.i.DropCard();
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
            ClientEventAggregator.Publish(new DropCardActionEvent(this));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.GetComponent<Image>().color = Color.clear;
            this.cardBigPicture.SetActive(true);
            this.cardBigPicture.transform.position = transform.position + new Vector3(0, this.GetComponent<RectTransform>().rect.height);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.GetComponent<Image>().color = Color.white;
            this.cardBigPicture.SetActive(false);
        }
    }
}
