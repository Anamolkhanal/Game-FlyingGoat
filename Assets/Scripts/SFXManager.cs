using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource sfxSource;
    public AudioClip jumpClip;
    public AudioClip moveClip;
    public AudioClip gameOverClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayJump()
    {
        sfxSource.PlayOneShot(jumpClip);
    }

    public void PlayMove()
    {
        sfxSource.PlayOneShot(moveClip);
    }

    public void PlayGameOver()
    {
        sfxSource.PlayOneShot(gameOverClip);
    }
}
