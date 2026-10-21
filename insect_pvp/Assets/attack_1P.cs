using System.Runtime.CompilerServices;
using UnityEngine;

public class attack_1P : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int MAX_HP_1P = 100;//ç≈ëÂHP

    private int currentHP_1P;//åªç›HP

    void Start()
    {
        currentHP_1P = MAX_HP_1P;

    }

    public void ToDamage_1P(int damage)
    {
        currentHP_1P -= damage;

        Debug.Log("1PécÇËHP"+currentHP_1P);

        if (currentHP_1P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
