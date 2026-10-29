using UnityEngine;

// holds all the sound clips and plays them
public class SoundManager : MonoBehaviour
{
    // static so any script (even prefabs) can play a sound
    public static SoundManager Instance;

    [Header("Clips")]
    public AudioClip chop;
    public AudioClip knottedHit;
    public AudioClip golden;
    public AudioClip pineconeFall;
    public AudioClip pineconeLand;
    public AudioClip death;

    private AudioSource source;

    void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    // play a clip once, lets sounds overlap
    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}
