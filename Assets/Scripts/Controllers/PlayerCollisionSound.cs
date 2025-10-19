using UnityEngine;
using System.Collections.Generic;

public class PlayerCollisionSound : MonoBehaviour
{
    [Header("Âm thanh khi va chạm")]
    [Tooltip("Âm thanh khi va chạm vào đá (Stone)")]
    public AudioClip stoneHitSound;

    [Tooltip("Âm thanh khi va chạm vào tuyết (Snow-Rock)")]
    public AudioClip snowRockHitSound;

    private AudioSource audioSource;

    // Danh sách lưu lại các chướng ngại vật đã phát âm
    private HashSet<GameObject> playedObjects = new HashSet<GameObject>();

    private void Start()
    {
        // Gắn AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.8f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        // Nếu vật này đã từng phát âm thanh → bỏ qua
        if (playedObjects.Contains(other))
            return;

        string name = other.name;
        Debug.Log("Player va chạm với: " + name);

        // Kiểm tra tên vật
        if (name.StartsWith("Stone"))
        {
            PlaySound(stoneHitSound);
            playedObjects.Add(other);
        }
        else if (name.StartsWith("Snow-Rock"))
        {
            PlaySound(snowRockHitSound);
            playedObjects.Add(other);
        }
    }

    // Phát âm thanh
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Thiếu AudioClip hoặc AudioSource để phát âm thanh!");
        }
    }
}
