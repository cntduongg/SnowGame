using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Các đối tượng trong game")]
    public GameObject mainMenuUI;       // Canvas hoặc Panel menu chính
    public Button playButton;           // Nút Play
    public GameObject player;           // Nhân vật Player
    public GameObject infoText;         // Text 1
    public GameObject text2;            // Text 2
    public GameObject instructionText;  // Text hướng dẫn

    private bool isGameStarted = false;
    private Vector3 startPosition;      // ✅ Lưu vị trí ban đầu của nhân vật
    private Quaternion startRotation;   // ✅ Lưu góc xoay ban đầu của nhân vật
    private bool startPositionSaved = false; // ✅ Đảm bảo chỉ lưu vị trí ban đầu một lần

    void Start()
    {
        // Dừng thời gian khi vào game, hiển thị menu
        Time.timeScale = 0f;

        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);

        if (player != null)
        {
            // ✅ Chỉ lưu vị trí và góc xoay ban đầu một lần duy nhất
            if (!startPositionSaved)
            {
                startPosition = player.transform.position;
                startRotation = player.transform.rotation;
                startPositionSaved = true;
                Debug.Log($"GameManager Start: LƯU VỊ TRÍ BAN ĐẦU của player = {startPosition}");
                Debug.Log($"GameManager Start: LƯU GÓC XOAY BAN ĐẦU của player = {startRotation}");
            }
            else
            {
                Debug.Log($"GameManager Start: Vị trí ban đầu đã được lưu = {startPosition}");
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
    }

    // 🔹 Khi nhấn nút Play
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
            Debug.Log($"StartGame: Player được bật, vị trí hiện tại = {player.transform.position}");
            
            // ✅ Đảm bảo player ở vị trí ban đầu
            ResetPlayerToStart();
        }

        Time.timeScale = 1f;
    }

    // ✅ Hàm riêng để reset player về trạng thái ban đầu
    private void ResetPlayerToStart()
    {
        if (player != null)
        {
            Debug.Log($"ResetPlayerToStart: Bắt đầu reset player");
            Debug.Log($"ResetPlayerToStart: Vị trí hiện tại = {player.transform.position}");
            Debug.Log($"ResetPlayerToStart: VỊ TRÍ BAN ĐẦU ĐƯỢC LƯU = {startPosition}");
            
            // Force reset position và rotation về vị trí ban đầu
            player.transform.position = startPosition;
            player.transform.rotation = startRotation;
            
            // Reset Rigidbody2D nếu có
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.rotation = 0f;
                Debug.Log("ResetPlayerToStart: Đã reset Rigidbody2D");
            }
            
            Debug.Log($"ResetPlayerToStart: SAU KHI RESET, vị trí = {player.transform.position}");
            Debug.Log($"ResetPlayerToStart: SAU KHI RESET, góc xoay = {player.transform.rotation}");
            
            // Kiểm tra xem có reset thành công không
            if (Vector3.Distance(player.transform.position, startPosition) < 0.01f)
            {
                Debug.Log("✅ ResetPlayerToStart: THÀNH CÔNG - Player đã về đúng vị trí ban đầu!");
            }
            else
            {
                Debug.LogError($"❌ ResetPlayerToStart: THẤT BẠI - Player không về đúng vị trí ban đầu! Khoảng cách = {Vector3.Distance(player.transform.position, startPosition)}");
            }
        }
    }

    // ✅ Hàm để manually set lại vị trí ban đầu (nếu cần)
    public void SetNewStartPosition()
    {
        if (player != null)
        {
            startPosition = player.transform.position;
            startRotation = player.transform.rotation;
            Debug.Log($"SetNewStartPosition: Đã set vị trí ban đầu mới = {startPosition}");
        }
    }

    // 🔹 Khi bị lật ngược hoặc chạm đá
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
        
        if (player != null)
        {
            // ✅ Reset player về trạng thái ban đầu
            ResetPlayerToStart();
            player.SetActive(false);
            Debug.Log("GameOver: Player đã được tắt");
        }
    }

    // ✅ Thêm hàm này để GameManager xử lý va chạm đá trực tiếp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu Player va vào đá (tag hoặc tên chứa "rock")
        if (collision.gameObject.CompareTag("Rock") ||
            collision.gameObject.name.ToLower().Contains("rock"))
        {
            Debug.Log("Player chạm đá! Game Over (xử lý trong GameManager)");
            GameOver();
        }
    }
}
