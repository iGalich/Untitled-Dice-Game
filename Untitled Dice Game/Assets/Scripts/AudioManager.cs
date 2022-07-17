using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private float[] _pitchRange = new float[2];
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound, float volume = 1f)
    {
        float _pitch = Random.Range(_pitchRange[0], _pitchRange[1]);
        _audioSource.pitch = _pitch;
        _audioSource.PlayOneShot(sound, volume);
    }
}