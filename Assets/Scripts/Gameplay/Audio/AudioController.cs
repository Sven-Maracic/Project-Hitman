
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] private AudioSource audioPlayerObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayAudio(AudioClip clip, Transform point, float volume)
    {
        AudioSource source = Instantiate(audioPlayerObject, point.position, Quaternion.identity);

        source.clip = clip;

        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;

        Destroy(source.gameObject, clipLength);
    }

    public AudioSource PlayLoopedAudio(AudioClip clip, Transform point, float volume)
    {
        AudioSource source = Instantiate(audioPlayerObject, point.position, Quaternion.identity);

        source.clip = clip;

        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;

        source.loop = true;
        return source;
    }
}