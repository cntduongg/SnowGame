using UnityEngine;
using TMPro;   // ← Quan trọng: để dùng TextMeshProUGUI

public class DistanceScore : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Transform player;               // Gán Player vào đây
    public TextMeshProUGUI scoreText;      // Gán Text TMP UI

    [Header("Cấu hình")]
    public float scoreMultiplier = 1f;     // Hệ số nhân điểm

    private float startX;                  // Vị trí ban đầu của Player
    private float currentScore;

    void Start()
    {
        if (player != null)
        {
            startX = player.position.x;    // Lưu vị trí ban đầu
        }

        currentScore = 0f;
        UpdateScoreText();
    }

    void Update()
    {
        if (player == null) return;

        // Tính khoảng cách theo trục X
        float distance = player.position.x - startX;

        // Không để điểm bị âm nếu player đi lùi
        if (distance < 0) distance = 0;

        currentScore = distance * scoreMultiplier;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(currentScore).ToString();
        }
    }

    public float GetScore()
    {
        return currentScore;
    }
}
