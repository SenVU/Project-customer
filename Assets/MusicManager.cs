using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip generalMusic;
    public AudioClip waterMusic;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayGeneralMusic();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterZone"))
        {
            PlayWaterMusic();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterZone"))
        {
            PlayGeneralMusic();
        }
    }

    void PlayGeneralMusic()
    {
        audioSource.clip = generalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayWaterMusic()
    {
        audioSource.clip = waterMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
