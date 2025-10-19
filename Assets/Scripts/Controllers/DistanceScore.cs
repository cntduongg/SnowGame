using UnityEngine;
using TMPro;   // ← để dùng TextMeshProUGUI

public class DistanceScore : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Transform player;                     // Gán Player vào đây
    public TextMeshProUGUI scoreText;            // Text hiển thị điểm hiện tại
    public TextMeshProUGUI highestScoreText;     // Text hiển thị điểm cao nhất

    [Header("Cấu hình")]
    public float scoreMultiplier = 1f;           // Hệ số nhân điểm

    private float startX;                        // Vị trí ban đầu của Player
    private float currentScore;
    private float highestScore;

    void Start()
    {
        if (player != null)
        {
            startX = player.position.x;          // Lưu vị trí ban đầu
        }

        // 🔸 Lấy Highest Score từ PlayerPrefs
        highestScore = PlayerPrefs.GetFloat("HighestScore", 0f);

        // 🔸 Cập nhật giao diện Highest Score
        if (highestScoreText != null)
        {
            highestScoreText.text = Mathf.FloorToInt(highestScore).ToString();
        }

        currentScore = 0f;
        UpdateScoreText();
    }

    void Update()
    {
        if (player == null) return;

        // 🔸 Tính khoảng cách theo trục X
        float distance = player.position.x - startX;
        if (distance < 0) distance = 0;

        currentScore = distance * scoreMultiplier;
        UpdateScoreText();

        // 🔸 Nếu đạt điểm cao mới → cập nhật & lưu lại
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetFloat("HighestScore", highestScore);
            PlayerPrefs.Save();

            if (highestScoreText != null)
            {
                highestScoreText.text = Mathf.FloorToInt(highestScore).ToString();
            }
        }
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
