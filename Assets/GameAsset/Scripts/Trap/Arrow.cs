using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    [SerializeField] private GameObject particleOnHitPrefabVFX;


    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (indestructible || player))
        {
            if ((player))
            {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (!other.isTrigger && indestructible)
            {
                Destroy(gameObject);
            }
        }
    }
}
