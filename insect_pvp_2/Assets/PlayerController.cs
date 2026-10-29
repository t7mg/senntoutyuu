using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;          //Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  //移動速度
    public float jump = 9.0f;   //ジャンプ力
    public LayerMask groundLayer;//着地できるレイヤー
    bool goJump = false;        //ジャンプ開始フラグ
    bool onGround = false;      //地面フラグ
    public int attack_powar = 5;       //攻撃力
    public float HP_1P = 100.0f;               //HP
    bool goattack_1P;                  //攻撃開始フラグ
    bool canattack;
    public float attack_cooltime = 60.0f;    //攻撃クールタイム

    private Player2PController player_2P_HP;


    void Start()
    {
        //Rigitbody2Dをとってくる
        rbody = this.GetComponent<Rigidbody2D>();
        goattack_1P = false;
        canattack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
          
            transform.localScale = new Vector2(-1, 1);//左右反転
        }
        else if (axisH < 0.0f)
        {
            //右移動
           
            transform.localScale = new Vector2(1, 1);//左右反転
        }

        //攻撃（S）の入力チェック
        if (Input.GetKeyDown(KeyCode.S))
        {
            canattack = true;

            if (canattack == true)
            {
                goattack_1P = true;
                Debug.Log("true");
                canattack = false;
            }
        }
        else
        {
            canattack = false;
 
        }

    }

    void FixedUpdate()
    {
        if (onGround || axisH != 0)  //地面の上or速度が0ではない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)      //地面の上でジャンプキーが押された
        {

            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false;                                 //ジャンプフラグを下す 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "2P" && goattack_1P == true)
        {

            if (collision.TryGetComponent<attack_2P>(out var ToDamage))
            {
                ToDamage.ToDamage_2P(attack_powar);
                Debug.Log("ダメージ成功");

            }

        }
        else
        {
            goattack_1P = false;
            //Debug.Log("敵が見つかりませんでした。");
        }

    }

    
   
   


}
