using UnityEngine;
using UnityEngine.Events;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Tooltip("Audio source where to play music")]
        public AudioSource music;

        [Tooltip("Event fired on first update tick")]
        public UnityEvent onStart;

        private bool isOnStart = true;

        public void Update()
        {
            if (isOnStart)
            {
                onStart.Invoke();
                isOnStart = false;
            }
        }

        public void PlayOneShot(AudioClip clip)
        {
            music.PlayOneShot(clip);
        }
    }
}
