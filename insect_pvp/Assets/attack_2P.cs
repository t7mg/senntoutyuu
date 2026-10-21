using System.Runtime.CompilerServices;
using UnityEngine;

public class attack_2P : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int MAX_HP_2P = 100;//ç≈ëÂHP

    private int currentHP_2P;//åªç›HP

    void Start()
    {
        currentHP_2P = MAX_HP_2P;

    }

    public void ToDamage_2P(int damage)
    {
        currentHP_2P -= damage;

        Debug.Log("2PécÇËHP"+currentHP_2P);

        if (currentHP_2P <= 0)
        {
            Destroy(gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}

    