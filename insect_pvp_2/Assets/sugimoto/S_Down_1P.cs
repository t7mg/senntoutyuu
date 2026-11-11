using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float speed = 0f;
    private bool isStopped = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isStopped) return;

        // 通常の移動
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // Is Trigger用の衝突検知
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_2")) 
        {
            if (!isStopped)
            {
                StartCoroutine(StopAndResumeRoutine());
            }
        }
    }

    // 1秒間だけ止まって、傾いて、また元に戻るコルーチン
    IEnumerator StopAndResumeRoutine()
    {
        isStopped = true;
        rb.linearVelocity = Vector2.zero;

        //敵の画像をZ軸を中心に45度傾ける（ピクッと倒れたような演出）
        transform.rotation = Quaternion.Euler(0, 0, 45f);

        //1秒間待つ
        yield return new WaitForSeconds(1.0f);

        // 敵の角度を 0度（元通り）に戻す
        transform.rotation = Quaternion.Euler(0, 0, 0);

        isStopped = false;                // 移動を再開
    }
}