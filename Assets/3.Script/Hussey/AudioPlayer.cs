using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ESFX sfx;

    public void PlaySFX()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = SoundManager.Instance.GetSFX(sfx);
            audioSource.Play();
        }
    }
}
