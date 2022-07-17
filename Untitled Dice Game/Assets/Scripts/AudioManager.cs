using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private float[] _pitchRange = new float[2];
    [SerializeField] private AudioSource _musicSource;
    private AudioSource[] _audioSources;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);

        _audioSources = GetComponents<AudioSource>();
    }

    public void PlaySound(AudioClip sound, float volume = 1f)
    {
        float _pitch = Random.Range(_pitchRange[0], _pitchRange[1]);
        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (_audioSources[i].isPlaying) continue;
            if (i == _audioSources.Length) i = 0;

            _audioSources[i].pitch = _pitch;
            _audioSources[i].PlayOneShot(sound, volume);
            break;
        }
    }

    public void SwitchMusic(AudioClip music)
    {
        _musicSource.clip = music;
        _musicSource.Play();
    }
}