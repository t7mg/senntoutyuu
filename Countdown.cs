using System.Collections;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    //ゲームが開始されたかどうかを管理するフラグ（外部から読み取れるように property にする）
    public bool isGameActive { get; private set; } = false;

    void Start()
    {
        // 最初は操作できないように false にしておく
        isGameActive = false;
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        countdownText.text = "START!";

        //「START!」のタイミングでゲーム開始フラグを true（操作可能）にする
        isGameActive = true;
        Debug.Log("ゲーム開始！操作を許可します。");

        yield return new WaitForSeconds(1.0f);
        countdownText.gameObject.SetActive(false);
    }
}