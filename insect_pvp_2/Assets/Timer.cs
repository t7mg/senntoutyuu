using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public int countdownMinutes = 3;
    private float countdownSeconds;
    private Text timeText;

    [SerializeField] private Countdown countdownScript;

    private void Start()
    {
        timeText = GetComponent<Text>();
        countdownSeconds = countdownMinutes * 60;

        //ゲーム開始前に、最初のタイマー表示（3:00）を画面に反映しておく
        var initialSpan = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = initialSpan.ToString(@"mm\:ss");
    }

    void Update()
    {
        // もしゲーム開始前（カウントダウン中）なら、タイマーを進めずにここで処理を終了する
        if (countdownScript != null && !countdownScript.isGameActive)
        {
            return;
        }

        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = span.ToString(@"mm\:ss");

        if (countdownSeconds <= 0)
        {
            Debug.Log("ゲーム終了！");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}