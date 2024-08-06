using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts.Client.BuildFeature
{
    public class ResourceBox : MonoBehaviour
    {
        public int Number { get; private set; }

        public SpriteAtlas SpriteAtlas;

        public SpriteRenderer NumberSpite;

        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public void SetNumber(int number)
        {
            this.Number = number;
            this.NumberSpite.sprite = this.SpriteAtlas.GetSprite(number.ToString());
        }
    }
}
