using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public enum EventSound { Say, LoseGame }

    [SerializeField] private AudioClip[] speaksPlayer;
    [SerializeField] private AudioClip loseGame;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
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
