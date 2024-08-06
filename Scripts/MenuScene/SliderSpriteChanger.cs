using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MenuScene
{
    [RequireComponent(typeof(Slider))]
    public class SliderSpriteChanger : MonoBehaviour
    {
        public Sprite[] Images;

        public Image TargetGameObjectImage;

        private Slider slider;

        void Awake()
        {
            this.slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(ChangeSprite);
        }

        private void ChangeSprite(float val)
        {
            var integerValue = Convert.ToInt32(val);
            TargetGameObjectImage.sprite = Images[integerValue];
        }
    }
}