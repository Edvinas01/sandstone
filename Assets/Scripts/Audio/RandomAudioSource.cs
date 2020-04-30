using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        [Tooltip("Min volume scale of randomized clips")]
        public float minVolumeScale = 0.8f;

        [Tooltip("Max volume scale of randomized clips")]
        public float maxVolumeScale = 1f;

        [Tooltip("Min pitch of randomized clips")]
        public float minPitch = 0.8f;

        [Tooltip("Max pitch of randomized clips")]
        public float maxPitch = 1f;

        [Tooltip("Clips to randomize")]
        public List<AudioClip> clips;

        private new AudioSource audio;

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
            var volumeScale = RandomUtils.GetRandom(minVolumeScale, maxVolumeScale);
            audio.pitch = RandomUtils.GetRandom(minPitch, maxPitch);
            audio.PlayOneShot(clip, volumeScale);
        }
    }
}
