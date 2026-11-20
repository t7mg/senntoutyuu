using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item_HP : MonoBehaviour
{
    public float cure = 25;

    static public float currentHP_1P = 100; // 現在HP


    public void OnCollisionEnter2D(Collision2D collision)
    {
        // collision.gameObject でぶつかった相手の情報を取得する
        if (collision.gameObject.CompareTag("1P"))
        {


            attack_1P player = collision.gameObject.GetComponent<attack_1P>();
            if (player != null)
            {
                attack_1P.currentHP_1P += 25;



                if (collision.gameObject.TryGetComponent<attack_1P>(out var ToCute))
                {
                    ToCute.ToCute(cure);
                    Debug.Log("1P残りHP" + currentHP_1P);


                }

                // アイテムを消滅させる
                Destroy(gameObject);

            }
        }

    }
}

