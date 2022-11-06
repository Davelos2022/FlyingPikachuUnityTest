using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Setting manager")]
    [SerializeField] private GameObject startDisplay;
    [SerializeField] private GameObject displayLose;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject playerPrefabs;
    [Header("Txt settings")]
    [SerializeField] private TextMeshProUGUI countGameTXT;
    [SerializeField] private TextMeshProUGUI resultTimeTXT;

    //Game
    private bool isGame; public bool IsGame => isGame;
    private int countGame;
    private int gameDifficulty;

    //Timer
    private float time;
    private int min;
    private int sec;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("CountGame"))
            countGame = PlayerPrefs.GetInt("CountGame");
        else
            countGame = 0;

        isGame = false;
    }
    private void Update()
    {
        if (isGame)
            Timer();
    }
    public void StartGame()
    {
        Instantiate(playerPrefabs, gamePanel.transform);
        countGame++;

        AudioManager.Instance?.PlaySound(AudioManager.EventSound.Say);

        isGame = true;
        time = 0;
        sec = 0;
        min = 0;
    }
    public void Timer()
    {
        time += Time.deltaTime;

        sec = (int)(time % 60);
        min = (int)(time / 60);
    }
    public void ClickPlay()
    {
        StartGame();

        startDisplay.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void ClickReplay()
    {
        StartGame();

        displayLose.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void ExitInMenu()
    {
        if (isGame)
        {
            isGame = false;
            gamePanel.SetActive(false);
            startDisplay.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoseGame()
    {
        isGame = false;
        countGameTXT.text = $"{countGame}";
        resultTimeTXT.text = $"{min} : {sec}";

        AudioManager.Instance?.PlaySound(AudioManager.EventSound.LoseGame);

        StartCoroutine(GameOver());
    }
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        gamePanel.SetActive(false);
        displayLose.SetActive(true);

        yield break;
    }
    public int CurrentDifficulty()
    {
        return gameDifficulty;
    }
    public void ClickDifficulty(int difficulty)
    {
        gameDifficulty = difficulty;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("CountGame", countGame);
    }
}
