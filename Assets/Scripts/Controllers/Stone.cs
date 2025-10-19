using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stone : MonoBehaviour
{
    [Tooltip("Event invoked when player collides with stone.")]
    public UnityEvent StoneCollisionEvent;

    [Tooltip("GameObjects to interact with.")]
    public GameObject[] TriggerCandidates;

    private HashSet<GameObject> triggerCandidates;

    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
    }

    // Trigger detection (cho trigger colliders)
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Stone: Trigger detected with {other.gameObject.name}");

        if (this.triggerCandidates.Contains(other.gameObject))
        {
            Debug.Log("Người chơi chạm vào đá! Invoking StoneCollisionEvent");
            this.StoneCollisionEvent.Invoke();

            // Backup: Gọi GameOver trực tiếp nếu UnityEvent không hoạt động
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Stone: Calling ShowGameOver() directly as backup");
                gm.ShowGameOver(); // ✅ đúng tên hàm
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
            Debug.Log("Người chơi chạm vào đá! (Collision method)");
            
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Stone: Calling GameOver via collision");
                gm.ShowGameOver();
            }
        }
    }
}
