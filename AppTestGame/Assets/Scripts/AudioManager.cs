using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum EventSound { Say, LoseGame }

    [SerializeField] private AudioClip[] speaksPlayer;
    [SerializeField] private AudioClip loseGame;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(EventSound typeSound)
    {
        switch (typeSound)
        {
            case EventSound.Say:
                audioSource.PlayOneShot(speaksPlayer[Random.Range(0, speaksPlayer.Length)]);
                break;
            case EventSound.LoseGame:
                audioSource.PlayOneShot(loseGame);
                break;
                
        }
    }
}
