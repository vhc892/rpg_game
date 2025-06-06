using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void Awake()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox triggered with: " + other.name);

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Debug.Log("Goblin hit the player!");
            player.TakeDamage(1, transform);
        }
    }
}
