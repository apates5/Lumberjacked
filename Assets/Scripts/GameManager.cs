using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// score, timer, high score, difficulty, game over + restart
public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Player player;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverText;
    public Image timerFill;

    [Header("Timer")]
    public float maxTime = 5f;
    public float timePerChop = 0.3f;   // time added back each chop
    public float drainRate = 1f;

    [Header("Golden Logs")]
    public int goldenPoints = 5;
    public float goldenTimeBonus = 1f;

    [Header("Difficulty")]
    public HazardSpawner hazardSpawner;
    public int chopsPerStage = 50;
    public float drainIncrease = 0.2f;
    public float maxDrainRate = 2.5f;
    public float hazardDelayMultiplier = 0.85f;
    public float minHazardDelay = 1.5f;

    private int chops = 0;
    private int stage = 1;

    private int score = 0;
    private int highScore = 0;
    private float timeLeft;
    private bool hasStarted = false;   // timer waits for the first chop
    private bool isGameOver = false;

    // used by the spawner so pinecones only fall mid-game
    public bool IsPlaying
    {
        get { return hasStarted && !isGameOver; }
    }

    void Start()
    {
        timeLeft = maxTime;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        gameOverText.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        // after game over only thing left is R to restart
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        // drain the timer, running out counts as dying
        if (hasStarted)
        {
            timeLeft -= drainRate * Time.deltaTime;

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                player.Die();
            }
        }

        timerFill.fillAmount = timeLeft / maxTime;
    }

    // called by the player whenever a log breaks
    public void LogChopped(LogKind kind)
    {
        hasStarted = true;

        // golden logs give more points and more time
        int points = 1;
        float timeBonus = timePerChop;
        if (kind == LogKind.Golden)
        {
            points = goldenPoints;
            timeBonus = goldenTimeBonus;
        }

        score += points;

        // every chopsPerStage logs the game gets harder
        chops++;
        if (chops % chopsPerStage == 0)
        {
            NextStage();
        }

        // add time back but don't go over max
        timeLeft = Mathf.Min(timeLeft + timeBonus, maxTime);

        UpdateUI();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // save new best score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        gameOverText.SetActive(true);
        UpdateUI();
    }

    // faster timer + more pinecones, both capped so it stays beatable
    void NextStage()
    {
        stage++;
        drainRate = Mathf.Min(drainRate + drainIncrease, maxDrainRate);
        hazardSpawner.minDelay = Mathf.Max(hazardSpawner.minDelay * hazardDelayMultiplier, minHazardDelay);
        hazardSpawner.maxDelay = Mathf.Max(hazardSpawner.maxDelay * hazardDelayMultiplier, minHazardDelay + 1f);
        Debug.Log("Stage " + stage);
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = "Best: " + highScore;
    }
}
