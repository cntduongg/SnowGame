
using UnityEngine;

public class CheatSystem : MonoBehaviour
{
    [Header("Cheat Settings")]
    [Tooltip("Tốc độ tăng lên khi bật cheat")]
    public float cheatSpeedMultiplier = 2.0f;

    [Tooltip("Lực nhảy tăng lên khi bật cheat")]
    public float cheatJumpMultiplier = 1.5f;

    [Tooltip("Bất tử khi bật cheat (không bị game over khi chạm đá)")]
    public bool invincible = true;

    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerJump playerJump;
    public GameManager gameManager;
  

    private bool cheatActive = false;

    private float originalMaxSpeed;
    private float originalJumpForce;

    private void Start()
    {
        // 🔍 Tự động tìm reference nếu chưa gán trong Inspector
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerJump == null)
            playerJump = FindObjectOfType<PlayerJump>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
       

        // 🧠 Clone ScriptableObjects để tránh ghi đè asset thật
        if (playerMovement != null)
        {
            playerMovement.MaxSpeed = Instantiate(playerMovement.MaxSpeed);
        }

        if (playerJump != null)
        {
            playerJump.JumpForce = Instantiate(playerJump.JumpForce);
        }

        // 💾 Lưu giá trị gốc để còn khôi phục
        if (playerMovement != null)
            originalMaxSpeed = playerMovement.MaxSpeed.Value;

        if (playerJump != null)
            originalJumpForce = playerJump.JumpForce.Value;

        // 🟢 Đảm bảo cheat tắt khi bắt đầu
        DeactivateCheat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleCheat();
        }
    }

    public void ToggleCheat()
    {
        cheatActive = !cheatActive;

        if (cheatActive)
        {
            ActivateCheat();
            Debug.Log("🔥 CHEAT ACTIVATED! Bạn đã bật cheat mode!");

      
        }
        else
        {
            DeactivateCheat();
            Debug.Log("🧊 Cheat deactivated. Trở về chế độ bình thường.");

          
        }
    }

    private void ActivateCheat()
    {
        // ⚡ Tăng tốc độ
        if (playerMovement != null)
        {
            playerMovement.MaxSpeed.SetValue(originalMaxSpeed * cheatSpeedMultiplier);
            Debug.Log($"Tốc độ tối đa tăng từ {originalMaxSpeed} lên {playerMovement.MaxSpeed.Value}");
        }

        // 🦘 Tăng lực nhảy
        if (playerJump != null)
        {
            playerJump.JumpForce.SetValue(originalJumpForce * cheatJumpMultiplier);
            Debug.Log($"Lực nhảy tăng từ {originalJumpForce} lên {playerJump.JumpForce.Value}");
        }

        if (gameManager != null)
            Debug.Log("Bật chế độ bất tử - không thể Game Over!");
    }

    private void DeactivateCheat()
    {
        // 🔁 Khôi phục tốc độ
        if (playerMovement != null)
        {
            playerMovement.MaxSpeed.SetValue(originalMaxSpeed);
            Debug.Log($"Khôi phục tốc độ tối đa về {originalMaxSpeed}");
        }

        // 🔁 Khôi phục lực nhảy
        if (playerJump != null)
        {
            playerJump.JumpForce.SetValue(originalJumpForce);
            Debug.Log($"Khôi phục lực nhảy về {originalJumpForce}");
        }

        Debug.Log("Tắt chế độ bất tử.");
    }

    public bool IsCheatActive()
    {
        return cheatActive;
    }

    public bool ShouldIgnoreGameOver()
    {
        return cheatActive && invincible;
    }
}