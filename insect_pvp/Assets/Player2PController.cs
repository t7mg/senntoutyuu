using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Player2PController : MonoBehaviour
{
    Rigidbody2D rbody;          //Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  //移動速度
    public float jump = 9.0f;   //ジャンプ力
    public LayerMask groundLayer;    //着地できるレイヤー
    bool goJump = false;        //ジャンプ開始フラグ
    bool onGround = false;      //地面フラグ
    public int attack_powar = 5;       //攻撃力
    public float HP_2P = 100.0f;               //HP
    bool goattack_2P;                  //攻撃開始フラグ
    bool canattack;
    public float attack_cooltime = 2.5f;    //攻撃クールタイム

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigitbody2Dをとってくる
        rbody = this.GetComponent<Rigidbody2D>();
        goattack_2P = false;
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
                                        groundLayer);            //検出するレイヤー

        if(Input.GetKey(KeyCode.UpArrow))
        {
            goJump = true;   //ジャンプフラグを立てる
        }
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
        if (axisH > 0.0f)
        {
            //右移動
            //Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);//左右反転
        }
        else if (axisH < 0.0f)
        {
            //右移動
           // Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);//左右反転
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            canattack = true;

            if (canattack == true)
            {
                goattack_2P = true;
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
        if(onGround && goJump)      //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false;                                 //ジャンプフラグを下す
        }
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "1P" && goattack_2P == true)
        {
            
            if(collision.TryGetComponent<attack_1P>(out var ToDamage))
            {
                ToDamage.ToDamage_1P(attack_powar);
                Debug.Log("ダメージ成功");
                
            }

        }
        else
        {
            goattack_2P = false;
            //Debug.Log("敵が見つかりませんでした。");
        }
    }

    
}
