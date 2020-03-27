using UnityEngine;
using Utils;

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

    private new AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audio.volume = RandomUtils.GetRandom(minVolume, maxVolume);
        audio.pitch = RandomUtils.GetRandom(minPitch, maxPitch);
        audio.clip = clip;

        audio.Play();
    }
}
