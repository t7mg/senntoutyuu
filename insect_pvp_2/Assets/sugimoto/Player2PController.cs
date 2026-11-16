using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;
using System.Collections;


public class Player2PController : MonoBehaviour
{
    [SerializeField] private Countdown countdownScript;
    Rigidbody2D rbody;          //Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  //移動速度
    public float jump = 9.0f;   //ジャンプ力
    public LayerMask groundLayer;    //着地できるレイヤー
    bool goJump = false;        //ジャンプ開始フラグ
    bool onGround = false;      //地面フラグ
    public float attack_powar = 5.0f;       //攻撃力
    //public float HP_2P = 100.0f;               //HP
    bool goattack_2P;                  //攻撃開始フラグ
    bool canattack;
    public float attack_cooltime;   //攻撃クールタイム
    public float Max_attack_cooltime = 300.0f;
    bool normal = false;
    public bool strong = false;
    public float strong_cooltime = 300.0f;
    public float stan_cooltime = 300.0f;
    public float current_stan_cooltime;

    public GameObject getstan1P;

    [Header("ダッシュ設定")]
    public float dashSpeed = 6.0f;             // ダッシュ時の移動速度
    public float doubleTapThreshold = 0.3f;    // 連打を許容する時間（秒）
    private float currentSpeed;                // 現在の適用速度
    private bool gofry = false;
    public float I_can_fry = 500.0f;
    public float frying_spped = 5.0f;
    private bool isDashing = false;            // ダッシュ中フラグ
    private float lastDTime = 0f;              // Dキーの前回タップ時間
    private float lastATime = 0f;              // Aキーの前回タップ時間
    private float lastUpTime = 0f;              // Wキーの前回タップ時間
    private KeyCode activeDashKey = KeyCode.None; // ダッシュを維持しているキー

    [Header("成長タイマー設定")]
    public float growthTimer = 60.0f; // 成長までの時間
    private bool hasGrown = false; // すでに成長したかどうかのフラグ7

    public bool getstan = true;

    [Header("ノックバック設定")]
    public float knockbackForce = 7.0f; // ノックバックの左右への強さ
    public float knockbackUpForce = 3.0f; // ノックバック時に少し浮き上がる強さ
    public float knockbackDuration = 0.4f; // 空中での操作不能時間（秒）
    public float landStunDuration = 1.0f; // 着地後の硬直時間（秒）
    public float invincibleDuration = 3.0f; //スタン復帰後の無敵時間（秒）

    [Header("ノックバック・スタン設定")]
    public bool isStopped = false;             // スタン中フラグ
    public bool isInvincible = false;          // 無敵時間フラグ
    private bool hasLandedDuringKnockback = false; // ノックバック中の着地フラグ

    public float diveBounceForceX = 4.0f;      // 敵を踏んだあとに後ろに下がる強さ
    public float diveBounceForceY = 6.0f;      // 敵を踏んだあとに上に跳ね上がる強さ


    [SerializeField] private InputAction _inputAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigitbody2Dをとってくる
        rbody = this.GetComponent<Rigidbody2D>();
        goattack_2P = false;
        canattack = false;
        currentSpeed = speed; // 初期速度は通常速度
        attack_cooltime = Max_attack_cooltime;
        getstan = GameObject.Find("kabutomusi_0");
        current_stan_cooltime = stan_cooltime;

    }

    // Update is called once per frame
    void Update()
    {
        // もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }

        // タイマー処理
        if (!hasGrown)
        {
            growthTimer -= Time.deltaTime;
            if (growthTimer <= 0f)
            {
                growth();
            }
        }

        if(getstan1P.GetComponent<PlayerController>().CanStun == true)
        {
            isStopped = true;
            getstan = false;
        }


        if (attack_cooltime > 0)
        {
            attack_cooltime--;
            // Debug.Log("attackNG");
        }
        else
        {
            attack_cooltime = 0;
            // Debug.Log("attackOK");
        }

        if (I_can_fry < 500.0f && gofry == false)
        {
            I_can_fry++;

        }

        if (isStopped == true)
        {
            current_stan_cooltime--;
        }
        if (current_stan_cooltime < 0)
        {
            isStopped = false;
            getstan = true;
            current_stan_cooltime = stan_cooltime;
        }

        //地上判定
        onGround = Physics2D.CircleCast(transform.position, //発射位置
                                        0.2f,               //円の半径
                                        Vector2.down,       //発射方向
                                        0.0f,               //発射距離
                                        groundLayer);            //検出するレイヤー

        if (isStopped)
        {
            axisH = 0f;
            return;
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            goJump = true;   //ジャンプフラグを立てる
        }

        if (gofry == true && I_can_fry != 0)
        {
            rbody.linearVelocity = new Vector2(rbody.linearVelocity.x, frying_spped);
            if (I_can_fry >= 0)
                I_can_fry--;
        }

        HandleDashInput();

        //水平方向の入力をチェックする
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            axisH = -1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            axisH = 1.0f;
        }
        else
        {
            axisH = 0.0f;
        }

        Vector3 currentScale = transform.localScale;
        if (axisH > 0.0f)
        {
            //右移動
            //Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);//左右反転
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (axisH < 0.0f)
        {
            //右移動
            // Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);//左右反転
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && attack_cooltime == 0)
        {
            canattack = true;
            attack_cooltime = Max_attack_cooltime;

            if (canattack == true)
            {


                goattack_2P = true;
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
            goattack_2P = false;
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
        if (onGround)
        {
            gofry = false;
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
                //Debug.Log("短押しされたよ！");
                normal = true;
                break;

            case HoldInteraction:
                // HoldInteractionの場合は長押し
                //Debug.Log("長押しされたよ！");
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
                //Debug.Log("通常移動に戻りました");
            }
        }
        else
        {
            // 右ダッシュ判定 Dキー
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Time.time - lastDTime <= doubleTapThreshold)
                {
                    isDashing = true;
                    activeDashKey = KeyCode.None; // 直後のバグ防止に一旦クリア
                    activeDashKey = KeyCode.LeftArrow;
                    currentSpeed = dashSpeed;
                    //Debug.Log("右ダッシュ開始！");
                }
                lastDTime = Time.time;
            }

            // 左ダッシュ判定 Aキー
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Time.time - lastATime <= doubleTapThreshold)
                {
                    isDashing = true;
                    activeDashKey = KeyCode.None;
                    activeDashKey = KeyCode.RightArrow;
                    currentSpeed = dashSpeed;
                    //Debug.Log("左ダッシュ開始！");
                }
                lastATime = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Time.time - lastUpTime <= doubleTapThreshold)
                {
                    isDashing = true;
                    activeDashKey = KeyCode.None;
                    activeDashKey = KeyCode.UpArrow;

                    if (gofry == true)
                    {
                        gofry = false;
                    }
                    else
                    {
                        gofry = true;
                    }
                    //Debug.Log("飛行開始！");
                }
                lastUpTime = Time.time;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController p1Script = this.gameObject.GetComponent<PlayerController>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1") && goattack_2P == true)
        {


            if (normal == true && hasGrown == true)
            {
                if (collision.TryGetComponent<attack_1P>(out var ToDamage))
                {
                    ToDamage.ToDamage_1P(attack_powar);
                    //Debug.Log("ダメージ成功a");

                    goattack_2P = false;
                    normal = false;
                }
            }

            else if (strong == true && hasGrown == true)
            {
                if (collision.TryGetComponent<attack_1P>(out var ToDamage))
                {
                    ToDamage.ToDamage_1P_strong(attack_powar);
                    //Debug.Log("ダメージ成功");

                    goattack_2P = false;
                    strong = false;
                }

            }
            else if (normal == true && hasGrown == false)
            {
                if (collision.TryGetComponent<attack_1P>(out var ToDamage))
                {
                    ToDamage.ToDamage_1P_baby(attack_powar);
                    //Debug.Log("ダメージ成功");
                    // play_SE.PlayOneShot(normal_SE);
                    goattack_2P = false;
                    normal = false;
                    //Debug.Log("omaeyowai");
                }
            }
            else if (strong == true && hasGrown == false)
            {
                if (collision.TryGetComponent<attack_1P>(out var ToDamage))
                {
                    ToDamage.ToDamage_1P_strong_baby(attack_powar);
                    //Debug.Log("ダメージ成功");
                    // play_SE.PlayOneShot(strong_SE);
                    goattack_2P = false;
                    strong = false;
                }
            }

        }
        else
        {

            goattack_2P = false;

            //Debug.Log("敵が見つかりませんでした。");
        }




    }

    void growth()
    {
        hasGrown = true;
        float directionX = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(1.2f * directionX, 1.2f, 1.2f);
    }

    //// 2Pから呼び出されるノックバックの受付メソッド
    //public void ApplyKnockback(float directionX, float forceX, float forceY, float duration, float stunDur, float invincDur)
    //{
    //    // 自分がすでに操作不能、または無敵時間中ならノックバックを無視する
    //    if (!isStopped && !isInvincible)
    //    {
    //        // 落下攻撃中にノックバックを食らった場合は、落下攻撃状態を強制解除

    //        StartCoroutine(KnockbackRoutine(directionX, forceX, forceY, duration, stunDur, invincDur));
    //    }
    //}

    //// ノックバックして一定時間操作不能＆傾けるコルーチン
    //private IEnumerator KnockbackRoutine(float directionX, float forceX, float forceY, float duration, float stunDur, float invincDur)
    //{
    //    isStopped = true;
    //    hasLandedDuringKnockback = false; // 着地フラグをリセット
    //    axisH = 0f;
    //    goJump = false;
    //    goattack_2P = false;
    //    // 初期ノックバック（上向きと横向きに弾き飛ばす）
    //    rbody.linearVelocity = Vector2.zero;
    //    Vector2 force = new Vector2(directionX * forceX, forceY);
    //    rbody.AddForce(force, ForceMode2D.Impulse);
    //    // プレイヤーの画像をZ軸を中心に45度傾ける
    //    transform.rotation = Quaternion.Euler(0, 0, 45f);
    //    // 地面に着地するか、通常の空中ノックバック時間が終わるまでループ待機
    //    float timer = 0f;
    //    while (timer < duration && !hasLandedDuringKnockback)
    //    {
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    // もし上のループが「着地」によって抜けた場合
    //    if (hasLandedDuringKnockback)
    //    {
    //        // 着地後の硬直カウント開始
    //        yield return new WaitForSeconds(stunDur);
    //    }
    //    // プレイヤーの角度を 0度（元通り）に戻す
    //    transform.rotation = Quaternion.Euler(0, 0, 0);
       
    //}
    //// 一定時間スタン（ノックバック）が効かなくなるコルーチン
    

    //// ノックバック中の着地検知
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    //落下攻撃中に敵当たった場合
    //    if ((other.gameObject.CompareTag("1P") || other.gameObject.layer == LayerMask.NameToLayer("1P")))
    //    {
    //        float currentDirX = -Mathf.Sign(transform.localScale.x);

    //        PlayerController p2Script = other.gameObject.GetComponent<PlayerController>();
    //        if (p2Script != null)
    //        {
    //            // 1Pの向き（currentDirX）と同じ方向へ2Pを吹き飛ばす
    //            float launchDirection = currentDirX;

    //            // 吹き飛ばす横の力、浮かせる縦の力、着地後の硬直秒数
    //            p2Script.ApplyDiveHitKnockback(launchDirection);


    //        }


    //        isStopped = false;

    //        // ぶつかった瞬間の速度を一度完全にリセット
    //        rbody.linearVelocity = Vector2.zero;

    //        // 跳ね返る方向は、進んでいた方向（currentDirX）の「逆」
    //        float bounceDirection = -currentDirX;

    //        Vector2 bounceVector = new Vector2(bounceDirection * diveBounceForceX, diveBounceForceY);
    //        rbody.AddForce(bounceVector, ForceMode2D.Impulse);

    //        return;

    //    }

    //    // 既存のノックバック中の着地判定
    //    if (isStopped && !hasLandedDuringKnockback && ((1 << other.gameObject.layer) & groundLayer) != 0)
    //    {
    //        hasLandedDuringKnockback = true;
    //        rbody.linearVelocity = new Vector2(0f, rbody.linearVelocity.y);
    //    }
    //}

    ////TapOrHoldExampleから短押しされた時に呼び出されるメソッド
    //public void OnAttackInputPressed()
    //{
    //    // 操作不能（スタン中など）なら入力を受け付けない
    //    if (isStopped) return;

    //    if (onGround)
    //    {
    //        // 地上にいるときは通常攻撃
    //        goattack_2P = true;
    //        Debug.Log("地上通常攻撃");
    //    }
    //    else
    //    {
    //        // 空中にいるときは、Wキーが押されていても無視して落下攻撃を発動
            
    //        isStopped = true; // 落下中は通常の左右移動やジャンプを受け付けない
    //        goJump = false;   // 空中でのジャンプ予約を強制クリア
    //        Debug.Log("空中落下攻撃");
    //    }
    //}

}