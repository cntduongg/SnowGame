using UnityEngine;

public class NoGravityCheat : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool cheatEnabled = false;
    private float originalGravity;

    [Header("Cấu hình bay")]
    public float riseDistance = 2f;        // khoảng bay lên lúc bật cheat
    public float riseSpeed = 5f;           // tốc độ bay lên
    public float forwardSpeed = 5f;        // tốc độ đi ngang (thẳng)
    public float speedStep = 2f;           // mức tăng/giảm tốc mỗi lần bấm phím

    private bool rising = false;
    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;  // lưu lại trọng lực gốc
    }

    void Update()
    {
        // Bấm L để bật/tắt cheat
        if (Input.GetKeyDown(KeyCode.L))
        {
            cheatEnabled = !cheatEnabled;

            if (cheatEnabled)
            {
                // Tắt trọng lực
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

                // Bắt đầu bay lên
                rising = true;
                targetPosition = new Vector2(transform.position.x, transform.position.y + riseDistance);
            }
            else
            {
                // Tắt cheat → khôi phục trọng lực
                rb.gravityScale = originalGravity;
                rising = false;
            }
        }

        if (cheatEnabled)
        {
            if (rising)
            {
                // Di chuyển lên tới vị trí mục tiêu
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetPosition,
                    riseSpeed * Time.deltaTime
                );

                if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
                {
                    rising = false; // đã bay lên xong, bắt đầu bay ngang
                }
            }
            else
            {
                // Bay ngang đều (theo trục X)
                transform.Translate(Vector2.right * forwardSpeed * Time.deltaTime);
            }

            // 👉 Điều chỉnh tốc độ bay khi cheat đang bật
            if (Input.GetKeyDown(KeyCode.K))
            {
                forwardSpeed += speedStep;
                Debug.Log("Tăng tốc: " + forwardSpeed);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                forwardSpeed = Mathf.Max(0, forwardSpeed - speedStep);
                Debug.Log("Giảm tốc: " + forwardSpeed);
            }
        }
    }
}
