using UnityEngine;

public class Goblin : MonoBehaviour, IEnemy
{
    [SerializeField] private float attackStopDistance = 1.2f;
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private Transform hitboxPivot;

    private Transform player;
    private EnemyPathfinding pathfinding;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        player = PlayerController.Instance.transform;
        pathfinding = GetComponent<EnemyPathfinding>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    public void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackStopDistance)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            pathfinding.MoveTo(dir);
        }
        else
        {
            pathfinding.StopMoving();

            bool isFacingLeft = player.position.x < transform.position.x;

            spriteRenderer.flipX = isFacingLeft;

            // ✅ Flip hitboxPivot (mirror X)
            if (hitboxPivot != null)
            {
                Vector3 scale = hitboxPivot.localScale;
                scale.x = isFacingLeft ? -1f : 1f;
                hitboxPivot.localScale = scale;
            }
            animator.SetTrigger("Attack");
        }
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }
}
