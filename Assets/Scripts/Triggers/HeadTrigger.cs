﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadTrigger : MonoBehaviour
{
    public BoolVariable IsAlive;
    public GameObject[] TriggerCandidates;

    private HashSet<GameObject> triggerCandidates;
    private CheatSystem cheatSystem;

    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
        this.cheatSystem = FindObjectOfType<CheatSystem>();
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

            Debug.Log("Head collision detected → Gọi GameOver()");
            GameManager.Instance.ShowGameOver(); // 👈 Gọi trực tiếp GameOver
        }
    }
}
