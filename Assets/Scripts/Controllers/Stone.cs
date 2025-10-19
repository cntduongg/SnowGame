using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stone : MonoBehaviour
{
    [Tooltip("Event invoked when player collides with stone.")]
    public UnityEvent StoneCollisionEvent;

    [Tooltip("GameObjects to interact with.")]
    public GameObject[] TriggerCandidates;
    
    [Header("Audio")]
    [Tooltip("Âm thanh khi va chạm vào đá")]
    public AudioClip collisionSound;
    
    [Tooltip("AudioSource để phát âm thanh")]
    public AudioSource audioSource;

    private HashSet<GameObject> triggerCandidates;
    private CheatSystem cheatSystem;

    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
        
        // Tìm CheatSystem
        this.cheatSystem = FindObjectOfType<CheatSystem>();
        
        // Tự động tạo AudioSource nếu chưa có
        if (this.audioSource == null)
        {
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
            this.audioSource.playOnAwake = false;
            this.audioSource.volume = 0.7f;
        }
    }

    // Trigger detection (cho trigger colliders)
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Stone: Trigger detected with {other.gameObject.name}");
        
        if (this.triggerCandidates.Contains(other.gameObject))
        {
            // Phát âm thanh va chạm
            PlayCollisionSound();
            
            Debug.Log("Người chơi chạm vào đá! Invoking StoneCollisionEvent");
            this.StoneCollisionEvent.Invoke();
            
            // Kiểm tra cheat system - nếu bật bất tử thì không game over
            if (this.cheatSystem != null && this.cheatSystem.ShouldIgnoreGameOver())
            {
                Debug.Log("Cheat active - Bỏ qua Game Over!");
                return;
            }
            
            // Backup: Gọi GameOver trực tiếp nếu UnityEvent không hoạt động
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Stone: Calling GameOver directly as backup");
                gm.GameOver();
            }
        }
        else
        {
            Debug.Log($"Stone: {other.gameObject.name} is not in trigger candidates");
        }
    }

    // Collision detection (cho collision colliders) - backup method
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Stone: Collision detected with {collision.gameObject.name}");
        
        // Kiểm tra nếu là Player hoặc có tag Player
        if (collision.gameObject.CompareTag("Player") || 
            this.triggerCandidates.Contains(collision.gameObject))
        {
            // Phát âm thanh va chạm
            PlayCollisionSound();
            
            Debug.Log("Người chơi chạm vào đá! (Collision method)");
            
            // Kiểm tra cheat system - nếu bật bất tử thì không game over
            if (this.cheatSystem != null && this.cheatSystem.ShouldIgnoreGameOver())
            {
                Debug.Log("Cheat active - Bỏ qua Game Over!");
                return;
            }
            
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Stone: Calling GameOver via collision");
                gm.GameOver();
            }
        }
    }
    
    // Phát âm thanh va chạm
    private void PlayCollisionSound()
    {
        if (this.audioSource != null && this.collisionSound != null)
        {
            this.audioSource.clip = this.collisionSound;
            this.audioSource.Play();
            Debug.Log("Phát âm thanh va chạm đá!");
        }
        else
        {
            Debug.LogWarning("Không thể phát âm thanh va chạm - thiếu AudioSource hoặc AudioClip!");
        }
    }
}
