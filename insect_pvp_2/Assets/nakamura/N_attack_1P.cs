using UnityEngine;
using UnityEngine.UI;
using System.Collections; // コルーチンを使うために必要

public class attack_1P : MonoBehaviour
{
    [SerializeField] private float MAX_HP_1P = 100; // 最大HP
    static public float currentHP_1P = 100; // 現在HP
    static public float baby_debuff = 2; // 幼虫時のデバフステータス

    [SerializeField] private Image _hpbarFill; // HPバーのImage

    private Coroutine dotCoroutine;

    void Start()
    {
        currentHP_1P = MAX_HP_1P;

        UpdateHPBar(); // 起動時にHPバーを最大にする
    }


    // 相手の攻撃プログラムから呼び出されるメソッド
    public void ToDamage_1P(float damage)
    {
        currentHP_1P -= damage;
        currentHP_1P = Mathf.Clamp(currentHP_1P, 0, MAX_HP_1P); // 0〜最大HPの間に収める


        Debug.Log("1P残りHP" + currentHP_1P);


        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_1P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }


    }

    public void ToDamage_1P_strong(float damage)
    {
        currentHP_1P -= damage * 2;
        currentHP_1P = Mathf.Clamp(currentHP_1P, 0, MAX_HP_1P); // 0〜最大HPの間に収める

        Debug.Log("1P残りHP" + currentHP_1P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_1P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }


    }

    public void ToDamage_1P_baby(float damage)
    {
        currentHP_1P -= damage / baby_debuff;
        currentHP_1P = Mathf.Clamp(currentHP_1P, 0, MAX_HP_1P); // 0〜最大HPの間に収める


        Debug.Log("1P残りHP" + currentHP_1P);


        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_1P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }


    }

    public void ToDamage_1P_strong_baby(float damage)
    {
        currentHP_1P -= damage * 2 / baby_debuff;
        currentHP_1P = Mathf.Clamp(currentHP_1P, 0, MAX_HP_1P); // 0〜最大HPの間に収める

        Debug.Log("1P残りHP" + currentHP_1P);

        UpdateHPBar(); // ダメージを受けたらHPバーを更新する

        if (currentHP_1P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
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

            currentHP_1P -= damage; // 1PのHPを減少させる
            Debug.Log($"継続ダメージ: {damage} 。残りHP_1P: {currentHP_1P}");
            UpdateHPBar(); // ダメージを受けたらHPバーを更新する

            if (currentHP_1P <= 0)
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
        
       _hpbarFill.fillAmount = (float)currentHP_1P / MAX_HP_1P;
          

    }

}