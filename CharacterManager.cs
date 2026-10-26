using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //カウントダウンスクリプトを参照するための変数
    [SerializeField] private Countdown countdownScript;
    [Header("キャラクターの登録")]
    public GameObject characterA;
    public GameObject characterB;

    private GameObject currentCharacter;

    void Start()
    {
        // 初期状態を設定（Aを表示、Bを非表示）
        characterA.SetActive(true);
        characterB.SetActive(false);
        currentCharacter = characterA;
    }

    void Update()
    {
        //もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }
        // E キーが押されたら交代処理を呼ぶ
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCharacter();
            Debug.Log("キャラクター変更");
        }
    }

    void SwitchCharacter()
    {
        if (currentCharacter == characterA)
        {
            // AからBへ交代
            // 交代先のBの位置と向きを、今のAと同じにする
            characterB.transform.position = characterA.transform.position;
            characterB.transform.rotation = characterA.transform.rotation;

            // Aを消してBを出す
            characterA.SetActive(false);
            characterB.SetActive(true);

            currentCharacter = characterB;
        }
        else
        {
            // BからAへ交代
            characterA.transform.position = characterB.transform.position;
            characterA.transform.rotation = characterB.transform.rotation;

            characterB.SetActive(false);
            characterA.SetActive(true);

            currentCharacter = characterA;
        }
    }
}