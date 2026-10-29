using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public float timePerChop = 0.3f;
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
    private bool hasStarted = false;
    private bool isGameOver = false;

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
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

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

    public void LogChopped(LogKind kind)
    {
        hasStarted = true;

        int points = 1;
        float timeBonus = timePerChop;
        if (kind == LogKind.Golden)
        {
            points = goldenPoints;
            timeBonus = goldenTimeBonus;
        }

        score += points;

        chops++;
        if (chops % chopsPerStage == 0)
        {
            NextStage();
        }

        timeLeft = Mathf.Min(timeLeft + timeBonus, maxTime);

        UpdateUI();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        gameOverText.SetActive(true);
        UpdateUI();
    }
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