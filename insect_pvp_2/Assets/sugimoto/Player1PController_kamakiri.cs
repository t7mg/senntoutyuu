using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class Player1PController_kamakiri : MonoBehaviour
{
    [SerializeField] private Countdown countdownScript;
    Rigidbody2D rbody;          //Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  //移動速度
    public float jump = 9.0f;   //ジャンプ力
    public LayerMask groundLayer;//着地できるレイヤー
    bool goJump = false;        //ジャンプ開始フラグ
    bool onGround = false;      //地面フラグ
    public int attack_powar = 5;       //攻撃力
    //public float HP_1P = 100.0f;               //HP
    bool goattack_1P;                  //攻撃開始フラグ
    bool canattack;
    public float attack_cooltime;    //攻撃クールタイム
    public float Max_attack_cooltime = 300.0f;
    bool normal = false;
    bool strong = false;
    public float strong_cooltime = 300.0f;

    [Header("ダッシュ、飛行設定")]
    public float dashSpeed = 6.0f;             // ダッシュ時の移動速度
    public float doubleTapThreshold = 0.3f;    // 連打を許容する時間（秒）
    private float currentSpeed;                // 現在の適用速度
    private bool isDashing = false;            // ダッシュ中フラグ
    private bool gofry = false;
    public float I_can_fry = 500.0f;
    public float frying_spped = 5.0f;
    //private bool isFrying = false;
    private float lastDTime = 0f;              // Dキーの前回タップ時間
    private float lastATime = 0f;              // Aキーの前回タップ時間
    private float lastWTime = 0f;              // Wキーの前回タップ時間
    private KeyCode activeDashKey = KeyCode.None; // ダッシュを維持しているキー
    //private KeyCode activeFryKey = KeyCode.None; // 飛行を維持しているキー



    [SerializeField] private InputAction _inputAction;
    void Start()
    {
        //Rigitbody2Dをとってくる
        rbody = this.GetComponent<Rigidbody2D>();
        goattack_1P = false;
        canattack = false;
        currentSpeed = speed; // 初期速度は通常速度
        attack_cooltime = Max_attack_cooltime;


    }


    void Update()
    {
        // もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }

        if (attack_cooltime > 0)
        {
            attack_cooltime--;

        }
        else
        {
            attack_cooltime = 0;

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
        if (axisH < 0.0f)
        {
            //右移動

            transform.localScale = new Vector2(-1, 1);//左右反転
        }
        else if (axisH > 0.0f)
        {
            //右移動

            transform.localScale = new Vector2(1, 1);//左右反転
        }

        //攻撃（S）の入力チェック
        if (Input.GetKeyDown(KeyCode.S) && attack_cooltime == 0)
        {
            canattack = true;
            attack_cooltime = Max_attack_cooltime;

            if (canattack == true)
            {


                goattack_1P = true;
                //Debug.Log("true");
                canattack = false;


            }
        }
        else
        {
            canattack = false;

        }

        if (attack_cooltime == Max_attack_cooltime - 5)
        {
            canattack = false;
            goattack_1P = false;
        }
    }

    void FixedUpdate()
    {
        // もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }

        if (onGround || axisH != 0)  //地面の上or速度が0ではない
        {
            //速度を更新する
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

    //長押し、短押し設定
    private void OnEnable()
    {
        // performedコールバックを登録
        _inputAction.performed += OnPerformed;

        // 入力を受け取るにはInputActionを有効化する必要がある
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        // InputActionを無効化
        _inputAction.Disable();

        // performedコールバックの登録を解除
        _inputAction.performed -= OnPerformed;
    }

    // InputActionのperformedイベント
    private void OnPerformed(InputAction.CallbackContext ctx)
    {
        // 短押しか長押しかの判別
        switch (ctx.interaction)
        {
            case TapInteraction:
                // TapInteractionの場合は短押し
                Debug.Log("短押しされたよ！");
                normal = true;
                break;

            case HoldInteraction:
                // HoldInteractionの場合は長押し
                Debug.Log("長押しされたよ！");
                strong = true;

                break;
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_2") && goattack_1P == true)
        {





            if (normal == true)
            {
                if (collision.TryGetComponent<attack_2P>(out var ToDamage))
                {
                    ToDamage.ToDamage_2P(attack_powar);
                    Debug.Log("ダメージ成功");

                    goattack_1P = false;
                    normal = false;
                }
            }
            else if (strong == true)
            {
                if (collision.TryGetComponent<attack_2P>(out var ToDamage))
                {
                    ToDamage.ToDamage_2P_strong(attack_powar);
                    Debug.Log("ダメージ成功");

                    goattack_1P = false;
                    strong = false;
                }
            }
            else
            {
                goattack_1P = false;
                //Debug.Log("敵が見つかりませんでした。");
            }

        }

    }
}







