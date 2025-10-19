using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [Tooltip("Âm thanh khi va chạm vào đá")]
    public AudioClip stoneCollisionSound;
    
    [Tooltip("Âm thanh khi nhảy")]
    public AudioClip jumpSound;
    
    [Tooltip("Âm thanh khi thắng")]
    public AudioClip victorySound;
    
    [Header("Audio Sources")]
    [Tooltip("AudioSource chính cho âm thanh game")]
    public AudioSource mainAudioSource;
    
    [Tooltip("AudioSource cho âm thanh va chạm")]
    public AudioSource collisionAudioSource;
    
    private static AudioManager instance;
    
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject audioManagerObject = new GameObject("AudioManager");
                    instance = audioManagerObject.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        // Đảm bảo chỉ có một AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // Tự động tạo AudioSource nếu chưa có
        if (mainAudioSource == null)
        {
            mainAudioSource = gameObject.AddComponent<AudioSource>();
            mainAudioSource.playOnAwake = false;
            mainAudioSource.volume = 0.8f;
        }
        
        if (collisionAudioSource == null)
        {
            collisionAudioSource = gameObject.AddComponent<AudioSource>();
            collisionAudioSource.playOnAwake = false;
            collisionAudioSource.volume = 0.7f;
        }
    }
    
    private void Start()
    {
        // Tự động gán âm thanh cho tất cả các đá trong scene
        AssignStoneCollisionSounds();
    }
    
    // Gán âm thanh va chạm cho tất cả các đá
    private void AssignStoneCollisionSounds()
    {
        Stone[] stones = FindObjectsOfType<Stone>();
        
        foreach (Stone stone in stones)
        {
            if (stone.collisionSound == null && stoneCollisionSound != null)
            {
                stone.collisionSound = stoneCollisionSound;
                Debug.Log($"Đã gán âm thanh va chạm cho đá: {stone.gameObject.name}");
            }
        }
        
        Debug.Log($"Đã gán âm thanh cho {stones.Length} đá trong scene");
    }
    
    // Phát âm thanh va chạm đá
    public void PlayStoneCollisionSound()
    {
        if (collisionAudioSource != null && stoneCollisionSound != null)
        {
            collisionAudioSource.clip = stoneCollisionSound;
            collisionAudioSource.Play();
        }
    }
    
    // Phát âm thanh nhảy
    public void PlayJumpSound()
    {
        if (mainAudioSource != null && jumpSound != null)
        {
            mainAudioSource.clip = jumpSound;
            mainAudioSource.Play();
        }
    }
    
    // Phát âm thanh thắng
    public void PlayVictorySound()
    {
        if (mainAudioSource != null && victorySound != null)
        {
            mainAudioSource.clip = victorySound;
            mainAudioSource.Play();
        }
    }
    
    // Điều chỉnh âm lượng
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        
        if (mainAudioSource != null)
            mainAudioSource.volume = volume * 0.8f;
        if (collisionAudioSource != null)
            collisionAudioSource.volume = volume * 0.7f;
    }
}
