using UnityEngine;
using UnityEngine.UI;

public class CheatUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Text hiển thị trạng thái cheat")]
    public Text cheatStatusText;
    
    [Tooltip("Panel chứa thông tin cheat")]
    public GameObject cheatInfoPanel;
    
    [Tooltip("Text hướng dẫn cheat")]
    public Text cheatInstructionText;
    
    [Header("Settings")]
    [Tooltip("Thời gian hiển thị thông báo cheat (giây)")]
    public float messageDisplayTime = 3f;
    
    private CheatSystem cheatSystem;
    private bool isShowingMessage = false;
    
    private void Start()
    {
        // Tìm CheatSystem
        this.cheatSystem = FindObjectOfType<CheatSystem>();
        
        // Ẩn panel thông tin cheat ban đầu
        if (this.cheatInfoPanel != null)
            this.cheatInfoPanel.SetActive(false);
            
        // Thiết lập text hướng dẫn
        if (this.cheatInstructionText != null)
        {
            this.cheatInstructionText.text = "Nhấn phím số 1 để bật/tắt CHEAT MODE\n" +
                                           "• Tăng tốc độ di chuyển\n" +
                                           "• Tăng lực nhảy\n" +
                                           "• Bất tử (không bị game over)";
        }
    }
    
    private void Update()
    {
        // Cập nhật trạng thái cheat
        UpdateCheatStatus();
        
        // Hiển thị hướng dẫn khi nhấn F1
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleCheatInfo();
        }
    }
    
    private void UpdateCheatStatus()
    {
        if (this.cheatSystem != null && this.cheatStatusText != null)
        {
            if (this.cheatSystem.IsCheatActive())
            {
                this.cheatStatusText.text = "CHEAT MODE: ON";
                this.cheatStatusText.color = Color.green;
            }
            else
            {
                this.cheatStatusText.text = "CHEAT MODE: OFF";
                this.cheatStatusText.color = Color.white;
            }
        }
    }
    
    private void ToggleCheatInfo()
    {
        if (this.cheatInfoPanel != null)
        {
            bool isActive = this.cheatInfoPanel.activeSelf;
            this.cheatInfoPanel.SetActive(!isActive);
        }
    }
    
    // Hiển thị thông báo tạm thời
    public void ShowTemporaryMessage(string message)
    {
        if (this.cheatStatusText != null && !this.isShowingMessage)
        {
            StartCoroutine(ShowMessageCoroutine(message));
        }
    }
    
    private System.Collections.IEnumerator ShowMessageCoroutine(string message)
    {
        this.isShowingMessage = true;
        string originalText = this.cheatStatusText.text;
        Color originalColor = this.cheatStatusText.color;
        
        this.cheatStatusText.text = message;
        this.cheatStatusText.color = Color.yellow;
        
        yield return new WaitForSeconds(this.messageDisplayTime);
        
        this.cheatStatusText.text = originalText;
        this.cheatStatusText.color = originalColor;
        this.isShowingMessage = false;
    }
}
