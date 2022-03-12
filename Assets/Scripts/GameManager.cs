using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Blaster blaster;
    private Centipede centipede;
    private MushroomField mushroomField;

    public GameObject gameOver;
    public Text scoreText;
    public Text livesText;

    private int score;
    private int lives;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        blaster = FindObjectOfType<Blaster>();
        centipede = FindObjectOfType<Centipede>();
        mushroomField = FindObjectOfType<MushroomField>();

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);

        blaster.Respawn();
        centipede.Respawn();
        mushroomField.Generate();
        gameOver.SetActive(false);
    }

    public void ResetRound()
    {
        SetLives(lives - 1);

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        blaster.Respawn();
        centipede.Respawn();
    }

    private void GameOver()
    {
        gameOver.SetActive(true);
        blaster.gameObject.SetActive(false);
    }

    public void IncreaseScore(int amount)
    {
        SetScore(score + amount);
    }

    public void NextLevel()
    {
        centipede.speed *= 1.1f;
        centipede.Respawn();

        mushroomField.amount = Mathf.CeilToInt(mushroomField.amount * 1.1f);
        mushroomField.Generate();

        blaster.Respawn();
    }

    private void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }

    private void SetLives(int value)
    {
        lives = value;
        livesText.text = lives.ToString();
    }

}
