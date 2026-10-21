using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //カウントダウンスクリプトを参照するための変数
    [SerializeField] private Countdown countdownScript;
    Rigidbody2D rbody;          //Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  //移動速度
    public float jump = 9.0f;   //ジャンプ力
    public LayerMask groundLayer;//着地できるレイヤー
    bool goJump = false;        //ジャンプ開始フラグ
    bool onGround = false;      //地面フラグ
    public float attack_powar = 5.0f;       //攻撃力
    public float HP_1P = 100.0f;               //HP
    bool goattack_1P = false;                  //攻撃開始フラグ
    public float attack_cooltime = 60.0f;    //攻撃クールタイム
    public bool canattack = true;

    [Header("ダッシュ設定")]
    public float dashSpeed = 6.0f;             // ダッシュ時の移動速度
    public float doubleTapThreshold = 0.3f;    // 連打を許容する時間（秒）
    private float currentSpeed;                // 現在の適用速度
    private bool isDashing = false;            // ダッシュ中フラグ
    private float lastDTime = 0f;              // Dキーの前回タップ時間
    private float lastATime = 0f;              // Aキーの前回タップ時間
    private KeyCode activeDashKey = KeyCode.None; // ダッシュを維持しているキー

    void Start()
    {
        //Rigidbody2Dをとってくる
        rbody = this.GetComponent<Rigidbody2D>();
        currentSpeed = speed; // 初期速度は通常速度
    }

    // Update is called once per frame
    void Update()
    {
        //もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }
        //地上判定
        onGround = Physics2D.CircleCast(transform.position, //発射位置
                                        0.2f,               //円の半径
                                        Vector2.down,       //発射方向
                                        0.0f,               //発射距離
                                        groundLayer);       //検出するレイヤー

        if (Input.GetKey(KeyCode.W))                    //キャラクターをジャンプさせる
        {
            goJump = true;   //ジャンプフラグを立てる
        }

        // --- ダッシュの入力チェック ---
        HandleDashInput();

        //A,Dの入力をチェックする
        if (Input.GetKey(KeyCode.A))
        {
            axisH = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            axisH = 1.0f;
        }
        else
        {
            axisH = 0.0f;
        }

        if (axisH > 0.0f)
        {
            //右移動
            //Debug.Log("右移動");
            transform.localScale = new Vector2(-1, 1);//左右反転
        }
        else if (axisH < 0.0f)
        {
            //左移動
            //Debug.Log("左移動");
            transform.localScale = new Vector2(1, 1);//左右反転
        }

        //攻撃（S）の入力チェック
        if (Input.GetKeyDown(KeyCode.S))
        {
            goattack_1P = true;
        }
    }

    void FixedUpdate()
    {
        if (onGround || axisH != 0)  //地面の上or速度が0ではない
        {
            // 速度を更新する
            rbody.linearVelocity = new Vector2(axisH * currentSpeed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)      //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false;                                 //ジャンプフラグを下す 
        }
    }

    // ダッシュの開始と終了を管理するメソッド
    void HandleDashInput()
    {
        if (isDashing)
        {
            // ダッシュのトリガーになったキーが離されたら通常速度に戻す
            if (Input.GetKeyUp(activeDashKey))
            {
                isDashing = false;
                activeDashKey = KeyCode.None;
                currentSpeed = speed;
                Debug.Log("通常移動に戻りました");
            }
        }
        else
        {
            // 右ダッシュ判定 Dキー
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastDTime <= doubleTapThreshold)
                {
                    isDashing = true;
                    activeDashKey = KeyCode.None; // 直後のバグ防止に一旦クリア
                    activeDashKey = KeyCode.D;
                    currentSpeed = dashSpeed;
                    Debug.Log("右ダッシュ開始！");
                }
                lastDTime = Time.time;
            }

            // 左ダッシュ判定 Aキー
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastATime <= doubleTapThreshold)
                {
                    isDashing = true;
                    activeDashKey = KeyCode.None;
                    activeDashKey = KeyCode.A;
                    currentSpeed = dashSpeed;
                    Debug.Log("左ダッシュ開始！");
                }
                lastATime = Time.time;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "2P" && goattack_1P == true)
        {
            Debug.Log("HP減少");

            if (HP_1P <= 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("2P");
                foreach (GameObject obj in objects)
                {
                    Debug.Log("削除");
                    Destroy(obj);
                }
            }
            goattack_1P = false;
        }
    }

    public void TakeDamage(int damage)
    {

    }
}