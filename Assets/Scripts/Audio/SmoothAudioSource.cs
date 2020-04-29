using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SmoothAudioSource : MonoBehaviour
    {
        [Tooltip("How fast should the volume decrease")]
        public float volumeRampDownSpeed = 4f;

        [Tooltip("How fast should the volume increase")]
        public float volumeRampUpSpeed = 2f;

        private new AudioSource audio;

        private float initialVolume;
        private float targetVolume;
        private float rampSpeed;

        /// <summary>
        /// Play the audio source by smoothly increasing its volume.
        /// </summary>
        public void Play()
        {
            targetVolume = initialVolume;
            rampSpeed = volumeRampUpSpeed;
        }

        /// <summary>
        /// Stop the audio source by smoothly decreasing its volume.
        /// </summary>
        public void Stop()
        {
            targetVolume = 0f;
            rampSpeed = volumeRampDownSpeed;
        }

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
            initialVolume = audio.volume;
            audio.volume = 0f;
        }

        private void Update()
        {
            if (targetVolume <= 0f && audio.volume <= 0f)
            {
                if (audio.isPlaying)
                {
                    audio.Pause();
                }
                return;
            }

            if (!audio.isPlaying)
            {
                audio.Play();
            }

            audio.volume = Mathf.MoveTowards(
                audio.volume,
                targetVolume,
                Time.deltaTime * rampSpeed
            );
        }
    }
}
