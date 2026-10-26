using System.Runtime.CompilerServices;
using UnityEngine;

public class attack_2P : MonoBehaviour
{
    [SerializeField] private int MAX_HP_2P = 100; // 最大HP
    public int currentHP_2P = 100; // 現在HP


    void Start()
    {
        currentHP_2P = MAX_HP_2P;

    }

    // 相手からの攻撃プログラムから呼び出されるメソッド
    public void ToDamage_2P(int damage)
    {
        currentHP_2P -= damage;

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}