using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysicsDashController : MonoBehaviour
{
    [Header("移動速度設定")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;

    [Header("ダブルタップ設定")]
    [SerializeField] private float doubleTapThreshold = 0.3f; // 連打を許容する時間（秒）

    private Rigidbody2D rb2d;
    private float currentSpeed;
    private bool isDashing = false;
    private float moveInput = 0f;

    // キーごとの前回タップ時間を管理
    private float lastDTime = 0f;
    private float lastATime = 0f;

    // 現在ダッシュのトリガーになっているキーを記録する用
    private KeyCode activeDashKey = KeyCode.None;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentSpeed = walkSpeed;

        // 物理移動時のキャラクターの不要な回転を防ぐ設定
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 1. 入力の取得
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. ダブルタップの判定
        HandleDoubleTapInput();

        // 3. ダッシュ中のキー離し判定
        if (isDashing)
        {
            if (Input.GetKeyUp(activeDashKey))
            {
                EndDash();
            }
        }
    }

    void FixedUpdate()
    {
        // 4. 実際の物理移動処理
        Move();
    }

    void HandleDoubleTapInput()
    {
        if (isDashing) return;

        // 右ダッシュ判定 Dキー
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastDTime <= doubleTapThreshold)
            {
                StartDash(KeyCode.D);
            }
            lastDTime = Time.time;
        }

        // 左ダッシュ判定 Aキー
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastATime <= doubleTapThreshold)
            {
                StartDash(KeyCode.A);
            }
            lastATime = Time.time;
        }
    }

    void StartDash(KeyCode key)
    {
        isDashing = true;
        activeDashKey = key;
        currentSpeed = dashSpeed;
    }

    void EndDash()
    {
        isDashing = false;
        activeDashKey = KeyCode.None;
        currentSpeed = walkSpeed;
    }

    void Move()
    {
        rb2d.linearVelocity = new Vector2(moveInput * currentSpeed, rb2d.linearVelocity.y);
    }
}