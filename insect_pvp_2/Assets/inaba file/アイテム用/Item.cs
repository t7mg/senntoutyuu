using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision.gameObject ‚Å‚Ô‚Â‚©‚Á‚½‘Šè‚Ìî•ñ‚ğæ“¾‚·‚é
        if (collision.gameObject.CompareTag("1P"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            { 
                player.growthTimer = 0;

            }

            // ƒAƒCƒeƒ€‚ğÁ–Å‚³‚¹‚é
            Destroy(gameObject);
        }
    }
}
