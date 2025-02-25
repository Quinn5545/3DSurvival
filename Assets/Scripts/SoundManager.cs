using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    // ----- SFX ----- //
    public AudioSource dropItemSound;
    public AudioSource pickupItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource treeFallSound;
    public AudioSource grassWalkSound;

    // ----- Music ----- //

    public AudioSource startingZoneBGMusic;
    public AudioSource startingZoneBGAmbience;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
