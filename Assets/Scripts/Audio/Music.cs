using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : Singleton<Music>
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.Play();
    }
}
