using Util;
using System.Collections.Generic;
using UnityEngine;

namespace Impact
{
    [RequireComponent(typeof(AudioSource))]
    public class ImpactAudioSource : MonoBehaviour
    {
        [Tooltip("Minimum volume scale of impact clips")]
        public float minVolumeScale = 0.8f;

        [Tooltip("Maximum volume scale of impact clips")]
        public float maxVolumeScale = 1f;

        [Tooltip("Minimum pitch of impact clips")]
        public float minPitch = 0.8f;

        [Tooltip("Maximum pitch of impact clips")]
        public float maxPitch = 1f;

        [Tooltip("Should a random offset be used for pitch and volume of each impact")]
        public bool isRandomOffset = true;

        [Tooltip("Random offset used for pitch and volume of each impact")]
        public float randomOffset = 0.07f;

        [Tooltip("Clips which will be played on hit")]
        public List<AudioClip> clips;

        private new AudioSource audio;

        public void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void PlayOnImpact(float strength)
        {
            if (strength <= 0)
            {
                return;
            }

            var clip = clips.GetRandom();
            if (clip == null)
            {
                return;
            }

            var volumeScale = GetVolume(strength);
            audio.pitch = GetPitch(strength);
            audio.PlayOneShot(clip, volumeScale);
        }

        private float GetOffset()
        {
            if (isRandomOffset)
            {
                return RandomUtils.GetRandom(-randomOffset, randomOffset);
            }

            return 0;
        }

        private float GetVolume(float strength)
        {
            var volume = (maxVolumeScale - minVolumeScale) * strength + GetOffset();

            return Mathf.Clamp(
                value: minVolumeScale + volume,
                min: minVolumeScale,
                max: maxVolumeScale
            );
        }

        private float GetPitch(float strength)
        {
            // Pitch is inverse, the higher the strength, the lower the pitch.
            var pitch = (maxPitch - minPitch) * (1 - strength) + GetOffset();

            return Mathf.Clamp(
                value: minPitch + pitch,
                min: minPitch,
                max: maxPitch
            );
        }
    }
}
