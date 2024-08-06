using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MusicManager : MonoBehaviour
    {
        [HideInInspector]
        public static MusicManager i;

        public AudioSource src;
        private readonly Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

        void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else if (i == this)
            {
                Destroy(gameObject);
            }

            var clips = Resources.LoadAll<AudioClip>("Sounds/");
            foreach (var clip in clips)
            {
                this.sounds.Add(clip.name, clip);
            }
        }

        public void Play(string soundKey)
        {
            this.src.clip = this.sounds[soundKey];
            this.src.Play();
        }
    }
}
