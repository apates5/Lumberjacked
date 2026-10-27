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

    private int score = 0;
    private int highScore = 0;
    private float timeLeft;
    private bool hasStarted = false;
    private bool isGameOver = false;

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

    public void AddScore(int points)
    {
        hasStarted = true;
        score += points;

        timeLeft = Mathf.Min(timeLeft + timePerChop, maxTime);

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

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = "Best: " + highScore;
    }
}