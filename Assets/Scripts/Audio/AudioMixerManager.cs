using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioMixerManager : MonoBehaviour
    {
        [Tooltip("Audio mixer to be managed")]
        public AudioMixer audioMixer;

        [Tooltip("Snapshot transition duration in seconds")]
        public float snapshotTransitionDuration = 5f;

        public string fadedOutSnapshotName;
        public string fadedInSnapshotName;

        private AudioMixerSnapshot fadeOut;
        private AudioMixerSnapshot fadeIn;

        public void TransitionFadeOut()
        {
            fadeOut.TransitionTo(snapshotTransitionDuration);
        }

        public void TransitionFadeIn()
        {
            fadeIn.TransitionTo(snapshotTransitionDuration);
        }

        private void Awake()
        {
            fadeOut = audioMixer.FindSnapshot(fadedOutSnapshotName);
            fadeIn = audioMixer.FindSnapshot(fadedInSnapshotName);
        }
    }
}
