using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Các đối tượng trong game")]
    public GameObject mainMenuUI;       // Menu chính
    public Button playButton;           // Nút Play
    public GameObject player;           // Nhân vật
    public TextMeshProUGUI infoText;    // Text 1
    public TextMeshProUGUI text2;       // Text 2
    public GameObject instructionText;  // Text hướng dẫn
    public GameObject timeCounterGo;    // Bộ đếm thời gian

    [Header("Game Over UI")]
    public GameObject gameOverObject;   // UI hoặc sprite Game Over

    private bool isGameStarted = false;
    private bool isGameOver = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool startPositionSaved = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0f;

        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);

        if (player != null)
        {
            if (!startPositionSaved)
            {
                startPosition = player.transform.position;
                startRotation = player.transform.rotation;
                startPositionSaved = true;
            }
            player.SetActive(false);
        }

        if (playButton != null)
            playButton.onClick.AddListener(StartGame);

        if (infoText != null)
            infoText.gameObject.SetActive(true);
        if (text2 != null)
            text2.gameObject.SetActive(true);
        if (instructionText != null)
            instructionText.SetActive(true);

        if (gameOverObject != null)
            gameOverObject.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Bắt đầu game!");

        isGameStarted = true;
        isGameOver = false;

        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);
        if (playButton != null)
            playButton.gameObject.SetActive(false);
        if (infoText != null)
            infoText.gameObject.SetActive(false);
        if (text2 != null)
            text2.gameObject.SetActive(false);
        if (instructionText != null)
            instructionText.SetActive(false);

        if (player != null)
        {
            player.SetActive(true);
            ResetPlayerToStart();
        }

        if (gameOverObject != null)
            gameOverObject.SetActive(false);

        Time.timeScale = 1f;

        if (timeCounterGo != null)
            timeCounterGo.GetComponent<TimeCounter>().StartTimeCounter();
    }

    private void ResetPlayerToStart()
    {
        if (player != null)
        {
            player.transform.position = startPosition;
            player.transform.rotation = startRotation;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.rotation = 0f;
            }
        }
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER!");

        if (gameOverObject != null)
        {
            // bật UI Image
            gameOverObject.SetActive(true);
        }

        // dừng game
        Time.timeScale = 0f;

        if (timeCounterGo != null)
            timeCounterGo.GetComponent<TimeCounter>().StopTimeCounter();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Khi GameOver thì bấm phím bất kỳ để restart lại
        if (isGameOver && Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
