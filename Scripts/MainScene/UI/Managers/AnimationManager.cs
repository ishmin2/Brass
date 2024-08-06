using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MainScene.UI
{
    public class AnimationManager : MonoBehaviour
    {
        [HideInInspector]
        public static AnimationManager instance;

        public Transform DeckPosition;
        public Transform PlayerHand;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
        }

        public async Task PlayerDrawCard()
        {
            var prefab = Resources.Load<GameObject>("Deck"); // TODO: replace string
            var card = Instantiate(prefab, this.DeckPosition);
            card.transform.localScale = new Vector3(1, 1);

            do
            {
                var from = card.transform.position;
                var to = Camera.main.ScreenToWorldPoint(PlayerHand.position);

                if (Vector2.Distance(from, to) > 3)
                {
                    card.transform.position = Vector3.MoveTowards(from, to, 0.2f);
                    await Task.Delay(Convert.ToInt16(Time.fixedDeltaTime * AnimationSpeed.Medium)); // TODO: get speed from config
                }
                else
                {
                    break;
                }
            } while (true);
            Destroy(card);
        }
    }
}
