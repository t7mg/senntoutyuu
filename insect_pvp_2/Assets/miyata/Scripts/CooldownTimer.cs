using UnityEngine;
using TMPro;

public class CooldownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float cooldownTime = 5f;

    private float remainingTime = 0f;

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 0)
                remainingTime = 0;

            timerText.text = remainingTime.ToString("F1");
        }
        else
        {
            timerText.text = "OK";
        }
    }
    public void StartCooldown()
    {
        remainingTime = Time.deltaTime;
    }
}
