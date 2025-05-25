using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip clip;
    public float volume = 1f;

    void Start()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        Destroy(gameObject, clip.length);
    }
}
