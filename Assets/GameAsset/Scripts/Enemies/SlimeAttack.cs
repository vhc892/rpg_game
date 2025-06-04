using UnityEngine;
using System.Collections;

public class SlimeAttack : MonoBehaviour, IEnemy
{
    private Rigidbody2D rb;
    private Transform player;
    private float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;

    private EnemyPathfinding pathfinding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance.transform;
        pathfinding = GetComponent<EnemyPathfinding>();
    }

    public void Attack()
    {
        Debug.Log("slime attack");
        if (!isDashing)
        {
            Vector2 dashDir = (player.position - transform.position).normalized;
            StartCoroutine(DashTowards(dashDir));
        }
    }

    private IEnumerator DashTowards(Vector2 direction)
    {
        isDashing = true;

        if (pathfinding != null)
            pathfinding.enabled = false;

        rb.velocity = Vector2.zero;

        Vector2 startPos = rb.position;
        Vector2 targetPos = player.position; // 👈 Dash đến vị trí người chơi

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, elapsed / dashDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPos); // đảm bảo đến đúng vị trí

        if (pathfinding != null)
            pathfinding.enabled = true;

        isDashing = false;
    }


}
