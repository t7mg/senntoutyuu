using UnityEngine;
using UnityEngine.UI; // 追加：UIを扱うために必要

public class attack_2P : MonoBehaviour
{
    [SerializeField] private float MAX_HP_2P = 100; // 最大HP
    static public float currentHP_2P = 100; // 現在HP
    static public float baby_debuff = 2.0f; // 幼虫時のデバフステータス
    [SerializeField] private Image _hpbarFill; // 2P用のHPバーのImage

    void Start()
    {
        currentHP_2P = MAX_HP_2P;
       
        UpdateHPBar(); // 起動時にHPバーを最大にする
    }

    void Update()
    {
       
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

    // HPバーの見た目を変化させるメソッド
    private void UpdateHPBar()
    {
        if (_hpbarFill == null) return;
        _hpbarFill.fillAmount = (float)currentHP_2P / MAX_HP_2P;
    }
}