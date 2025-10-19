using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadTrigger : MonoBehaviour
{
    public BoolVariable IsAlive;

    [Tooltip("Event invoked when collision occurs.")]
    public UnityEvent HeadCollisionEvent;

    [Tooltip("GameObjects to interact with.")]
    public GameObject[] TriggerCandidates;

    private HashSet<GameObject> triggerCandidates;
    private CheatSystem cheatSystem; // 👈 Thêm biến tham chiếu cheat

    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
        this.cheatSystem = FindObjectOfType<CheatSystem>(); // 👈 Tìm đối tượng cheat
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.triggerCandidates.Contains(other.gameObject) && this.IsAlive.Value)
        {
            if (this.cheatSystem != null && this.cheatSystem.ShouldIgnoreGameOver())
            {
                Debug.Log("Cheat active - Bỏ qua va chạm đầu!");
                return;
            }

            Debug.Log("Head collision detected! Invoking HeadCollisionEvent");
            this.HeadCollisionEvent.Invoke();
        }
    }
}
