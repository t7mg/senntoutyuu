using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Countdown countdownScript;

    [Header("出現させるアイテム")]
    public GameObject[] itemPrefabs;

    [Header("出現範囲の設定")]
    public float minX = -7f; // 左端の座標
    public float maxX = 7f;  // 右端の座標
    public float spawnY = 5f; // 出現する高さ（Y座標）

    [Header("出現間隔")]
    public float spawnInterval = 5f;

    private float timer;

    void Update()
    {
        // もしゲームが開始されていなければ、これ以降の移動処理を実行しない
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }

        timer += Time.deltaTime;

        // 指定した秒数が経過したらアイテムを出現させる
        if (timer >= spawnInterval)
        {
            SpawnRandomItem();
            timer = 0f;
        }
    }

    void SpawnRandomItem()
    {
        if (itemPrefabs.Length == 0) return;

        // ランダムなX座標を決定
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, spawnY);

        // 登録されたプレハブからランダムに1つ選ぶ
        int randomIndex = Random.Range(0, itemPrefabs.Length);

        // アイテムを生成
        Instantiate(itemPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }
}
