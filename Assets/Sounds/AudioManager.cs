using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioClip backgroundMusic;
    public AudioClip bossMusic;
    public AudioClip bossDeath;
    public AudioClip playerHit;
    public AudioClip whatIsDead;
    public AudioClip bossHit;
    public AudioClip playerGetsHit;

    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    public void PlayMusic(AudioClip clip)
    {
        // Prevent restarting the same track
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void ChangeToBossMusic()
    {
        PlayMusic(bossMusic);
    }
}