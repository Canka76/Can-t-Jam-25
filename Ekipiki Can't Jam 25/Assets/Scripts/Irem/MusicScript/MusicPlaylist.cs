using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] musicClips; // Müzik listesi
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Sahne geçiþinde kaybolmasýn
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayCurrentTrack();
    }

    private void Update()
    {
        // Þarký bittiðinde bir sonraki þarkýya geç
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
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length; // Döngüsel ilerleme
        PlayCurrentTrack();
    }
}
