using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;
    private Shooter shooter;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shooter = GetComponent<Shooter>();
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        if (shooter == null || !shooter.IsAttacking)
        {
            if (moveDir.x < 0) spriteRenderer.flipX = true;
            else if (moveDir.x > 0) spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPositon)
    {
        moveDir = targetPositon;
    }
    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }

    
}
