using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Các đối tượng trong game")]
    public GameObject mainMenuUI;       // Canvas hoặc Panel menu chính
    public Button playButton;           // Nút Play
    public GameObject player;           // Nhân vật Player
    public GameObject infoText;         // Text 1
    public GameObject text2;            // Text 2
    public GameObject instructionText; // Text hướng dẫn
    public GameObject TimeCounterGo;

    [Header("Game Over UI")]
    public GameObject gameOverObject;   // Sprite hoặc UI hiển thị "GAME OVER"

    private bool isGameStarted = false;
    private Vector3 startPosition;      // Lưu vị trí ban đầu của nhân vật
    private Quaternion startRotation;   // Lưu góc xoay ban đầu của nhân vật
    private bool startPositionSaved = false; // Đảm bảo chỉ lưu vị trí ban đầu một lần
    void Start()
    {
        // Dừng thời gian khi vào game, hiển thị menu
        Time.timeScale = 0f;

        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);

        if (player != null)
        {
            // Chỉ lưu vị trí và góc xoay ban đầu một lần duy nhất
            if (!startPositionSaved)
            {
                startPosition = player.transform.position;
                startRotation = player.transform.rotation;
                startPositionSaved = true;
                Debug.Log($"GameManager Start: Lưu vị trí ban đầu = {startPosition}");
                Debug.Log($"GameManager Start: Lưu góc xoay ban đầu = {startRotation}");
            }
            player.SetActive(false);
        }

        if (playButton != null)
            playButton.onClick.AddListener(StartGame);

        if (infoText != null)
            infoText.SetActive(true);
        if (text2 != null)
            text2.SetActive(true);
        if (instructionText != null)
            instructionText.SetActive(true);

        // Ẩn GameOver lúc đầu
        if (gameOverObject != null)
            gameOverObject.SetActive(false);
    }

    // Khi nhấn nút Play
    public void StartGame()
    {
        Debug.Log("Bắt đầu hoặc chơi lại từ đầu!");
        isGameStarted = true;

        // Ẩn menu, text, bật player
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);
        if (playButton != null)
            playButton.gameObject.SetActive(false);
        if (infoText != null)
            infoText.SetActive(false);
        if (text2 != null)
            text2.SetActive(false);
        if (instructionText != null)
            instructionText.SetActive(false);

        if (player != null)
        {
            player.SetActive(true);
            ResetPlayerToStart();
        }

        // Ẩn sprite GameOver nếu đang hiện
        if (gameOverObject != null)
            gameOverObject.SetActive(false);

        Time.timeScale = 1f;

        // Bắt đầu đếm thời gian khi game bắt đầu
        if (TimeCounterGo != null)
        {
            TimeCounterGo.GetComponent<TimeCounter>().StartTimeCounter();
        }

    }

    // Reset player về vị trí ban đầu
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

            Debug.Log("ResetPlayerToStart: Player đã được reset về vị trí ban đầu");
        }
    }

    // Set lại vị trí ban đầu thủ công (nếu cần)
    public void SetNewStartPosition()
    {
        if (player != null)
        {
            startPosition = player.transform.position;
            startRotation = player.transform.rotation;
            Debug.Log($"SetNewStartPosition: Đã set vị trí ban đầu mới = {startPosition}");
        }
    }

    // Khi bị lật ngược hoặc chạm đá
    public void GameOver()
    {
        Debug.Log("Game Over! Quay lại menu.");
       
        isGameStarted = false;
        Time.timeScale = 0f;

        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);
        if (playButton != null)
            playButton.gameObject.SetActive(true);
        if (infoText != null)
            infoText.SetActive(true);
        if (text2 != null)
            text2.SetActive(true);
        if (instructionText != null)
            instructionText.SetActive(true);

        // Hiện sprite GameOver
        if (gameOverObject != null)
            gameOverObject.SetActive(true);

        if (player != null)
        {
            ResetPlayerToStart();
            player.SetActive(false);
        }

        // Dừng đếm thời gian khi game over
        if (TimeCounterGo != null)
        {
            TimeCounterGo.GetComponent<TimeCounter>().StopTimeCounter();
        }

    }

    // Xử lý va chạm trực tiếp nếu Player có collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock") ||
            collision.gameObject.name.ToLower().Contains("rock"))
        {
            Debug.Log("Player chạm đá! Game Over (xử lý trong GameManager)");
            GameOver();
        }
    }

    // (Tuỳ chọn) Restart toàn bộ Scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
