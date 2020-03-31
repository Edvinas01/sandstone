using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        [Tooltip("Min volume of randomized clips")]
        public float minVolume = 0.8f;

        [Tooltip("Max volume of randomized clips")]
        public float maxVolume = 1f;

        [Tooltip("Min pitch of randomized clips")]
        public float minPitch = 0.8f;

        [Tooltip("Max pitch of randomized clips")]
        public float maxPitch = 1f;

        [Tooltip("Clips to randomize")]
        public List<AudioClip> clips;

        private AudioSource audio;

        public void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void Play()
        {
            Play(clips.GetRandom());
        }

        public void Play(AudioClip clip)
        {
            audio.volume = RandomUtils.GetRandom(minVolume, maxVolume);
            audio.pitch = RandomUtils.GetRandom(minPitch, maxPitch);
            audio.clip = clip;

            audio.Play();
        }
    }
}
