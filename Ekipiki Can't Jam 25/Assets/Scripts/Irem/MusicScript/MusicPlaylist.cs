using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] musicClips; // M�zik listesi
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Sahne ge�i�inde kaybolmas�n
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayCurrentTrack();
    }

    private void Update()
    {
        // �ark� bitti�inde bir sonraki �ark�ya ge�
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayCurrentTrack()
    {
        if (musicClips.Length == 0) return;

        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();
    }

    void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length; // D�ng�sel ilerleme
        PlayCurrentTrack();
    }
}
