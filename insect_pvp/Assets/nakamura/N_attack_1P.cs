//using System.Runtime.CompilerServices;
//using UnityEngine;
//
//public class attack_1P : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//
//    [SerializeField] private int MAX_HP_1P = 100;//最大HP
//
//    [SerializeField] static public int currentHP_1P = 100;//現在HP
//
//    void Start()
//    {
//        currentHP_1P = MAX_HP_1P;
//
//    }
//
//    public void ToDamage_1P(int damage)
//    {
//        currentHP_1P -= damage;
//
//        Debug.Log("1P残りHP"+currentHP_1P);
//
//        if (currentHP_1P <= 0)
//        {
//            Destroy(gameObject);
//            UnityEditor.EditorApplication.isPlaying = false;
//        }
//    }
//}
//

using UnityEngine;
using UnityEngine.UI;

public class attack_1P : MonoBehaviour
{
    [SerializeField] private int MAX_HP_1P = 100; // 最大HP
    public int currentHP_1P = 100; // 現在HP
    [SerializeField] private Image _hpbarFill; // HPバーのImage

    void Start()
    {
        currentHP_1P = MAX_HP_1P;
        UpdateHPBar(); // 起動時にHPバーを最大にする
    }

    // 相手の攻撃プログラムから呼び出されるメソッド
    public void ToDamage_1P(int damage)
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

    // HPバーの見た目を変化させるメソッド
    private void UpdateHPBar()
    {
        if (_hpbarFill == null) return;
        _hpbarFill.fillAmount = (float)currentHP_1P / MAX_HP_1P;
    }
}
