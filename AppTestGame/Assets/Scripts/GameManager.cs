using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Setting manager")]
    [SerializeField] private GameObject StartDisplay;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject DisplayLose;
    [SerializeField] private GameObject PlayerPrefabs;
    [Header("Txt settings")]
    [SerializeField] private TextMeshProUGUI countGameTXT;
    [SerializeField] private TextMeshProUGUI resultTimeTXT;

    //Game
    private bool isGame; public bool IsGame => isGame;
    private int _countGame;
    private int _gameDifficulty;

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
            _countGame = PlayerPrefs.GetInt("CountGame");
        else
            _countGame = 0;

        isGame = false;
    }
    private void Update()
    {
        if (isGame)
            Timer();
    }
    public void StartGame()
    {
        Instantiate(PlayerPrefabs, GamePanel.transform);
        _countGame++;

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
    public void PlayGame()
    {
        StartGame();

        StartDisplay.SetActive(false);
        GamePanel.SetActive(true);
    }
    public void Replay()
    {
        StartGame();

        DisplayLose.SetActive(false);
        GamePanel.SetActive(true);
    }
    public void LoseGame()
    {
        isGame = false;
        countGameTXT.text = $"{_countGame}";
        resultTimeTXT.text = $"{min} : {sec}";

        StartCoroutine(GameOver());
    }
    public int CurrentDifficulty()
    {
        return _gameDifficulty;
    }
    public void ClickDifficulty(int difficulty)
    {
        _gameDifficulty = difficulty;
    }
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        GamePanel.SetActive(false);
        DisplayLose.SetActive(true);
    }
    public void ExitInMenu()
    {
        if (isGame)
        {
            isGame = false;
            GamePanel.SetActive(false);
            StartDisplay.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("CountGame", _countGame);
    }
}
