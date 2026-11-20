using UnityEngine;
using UnityEngine.UI; // 追加：UIを扱うために必要
using System.Collections; // コルーチンを使うために必要

public class attack_2P : MonoBehaviour
{
    [SerializeField] private float MAX_HP_2P = 100; // 最大HP
    static public float currentHP_2P = 100; // 現在HP
    static public float baby_debuff = 2.0f; // 幼虫時のデバフステータス
    [SerializeField] private Image _hpbarFill; // 2P用のHPバーのImage

    public GameObject getstan2P;

    public bool CanStun = false;

    private Coroutine dotCoroutine;

    void Start()
    {
        currentHP_2P = MAX_HP_2P;
       
        UpdateHPBar(); // 起動時にHPバーを最大にする
    }

 

    // 相手からの攻撃プログラムから呼び出されるメソッド
    public void ToDamage_2P(float damage)
    {
        currentHP_2P -= damage;
        currentHP_2P = Mathf.Clamp(currentHP_2P, 0, MAX_HP_2P); // 0〜最大HPの間に収める
        

        Debug.Log("2P残りHP" + currentHP_2P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
        
    }

    public void ToDamage_2P_strong(float damage)
    {
        currentHP_2P -= damage*2;
        currentHP_2P = Mathf.Clamp(currentHP_2P, 0, MAX_HP_2P); // 0〜最大HPの間に収める

        Debug.Log("2P残りHP" + currentHP_2P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void ToDamage_2P_baby(float damage)
    {
        currentHP_2P -= damage / baby_debuff;
        currentHP_2P = Mathf.Clamp(currentHP_2P, 0, MAX_HP_2P); // 0〜最大HPの間に収める


        Debug.Log("2P残りHP" + currentHP_2P);

        //if (getstan2P.GetComponent<Player2PController>().getstan == false)
        //{
        //    CanStun = false;
        //}

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }


    }

    public void ToDamage_2P_strong_baby(float damage)
    {
        currentHP_2P -= damage * 2 / baby_debuff;
        currentHP_2P = Mathf.Clamp(currentHP_2P, 0, MAX_HP_2P); // 0〜最大HPの間に収める

        Debug.Log("2P残りHP" + currentHP_2P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }


    }

    public void ToCute(float cure)
    {
        currentHP_2P += cure;
        Debug.Log("1P残りHP" + currentHP_2P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

    }

    // 2Pから呼び出される継続ダメージの受付メソッド
    public void ApplyDoT(int damagePerTick, float interval, int tickCount)
    {
        //すでに毒状態（コルーチンが実行中）なら、新しい付与を無視する
        if (dotCoroutine != null)
        {
            Debug.Log("毒付与無し");
            return;
        }

        // 毒がついていない時だけ、新しくコルーチンを開始する
        dotCoroutine = StartCoroutine(DoTLoop(damagePerTick, interval, tickCount));
    }

    // 継続ダメージの内部処理（HP_1Pを減らす）
    private IEnumerator DoTLoop(int damage, float interval, int ticks)
    {
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(interval);

            currentHP_2P -= damage; // 1PのHPを減少させる
            Debug.Log($"継続ダメージ: {damage} 。残りHP_1P: {currentHP_2P}");
            UpdateHPBar(); // ダメージを受けたらHPバーを更新する

            if (currentHP_2P <= 0)
            {

                Destroy(gameObject);
                UnityEditor.EditorApplication.isPlaying = false;
                // 死亡した場合はこれ以上ダメージ処理をしないようにループを抜ける
                break;
            }
        }

        // すべてのTickが終わるか、途中でループを抜けたら変数を初期化
        dotCoroutine = null;
    }

    // HPバーの見た目を変化させるメソッド
    private void UpdateHPBar()
    {
        if (_hpbarFill == null) return;
        _hpbarFill.fillAmount = (float)currentHP_2P / MAX_HP_2P;
    }
}