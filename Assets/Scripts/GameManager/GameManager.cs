using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Button playButton;        // Nút Play trong Canvas
    public GameObject mainMenuUI;    // Giao diện menu chính (Panel / Canvas)

    [Header("Gameplay")]
    public GameObject player;        // Người chơi hoặc nhân vật chính
    public GroundSpeed ground;       // Script điều khiển tốc độ mặt đất

    private bool isGameStarted = false;

    void Start()
    {
        // Khi vào game, tạm dừng
        isGameStarted = false;
        Time.timeScale = 0f;

        // Gán sự kiện cho nút Play
        playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;
        Debug.Log("Game Started!");

        // Ẩn menu chính (nếu có)
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);

        // Ẩn luôn nút Play sau khi nhấn
        if (playButton != null)
            playButton.gameObject.SetActive(false);

        // Kích hoạt nhân vật
        if (player != null)
            player.SetActive(true);

        // Bật tốc độ mặt đất
        if (ground != null)
            ground.SetGroundSpeed(ground.DefaultSpeed.Value);

        // Bắt đầu game
        Time.timeScale = 1f;
    }
}
