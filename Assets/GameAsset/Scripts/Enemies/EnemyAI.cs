﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }
    private void Start()
    {
        Debug.Log("stopMovingWhileAttacking = " + stopMovingWhileAttacking);
        roamPosition = GetRoamingPosition();
    }
    private void Update()
    {
        MovementStateControl();
    }
    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }
    private void Roaming()
    {
        if (GameModeManager.Instance.CurrentMode == GameMode.Survival)
        {
            Vector2 dirToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            enemyPathfinding.MoveTo(dirToPlayer);
        }
        else
        {
            //MainGame random roaming
            timeRoaming += Time.deltaTime;
            enemyPathfinding.MoveTo(roamPosition);

            if (timeRoaming > roamChangeDirFloat)
            {
                roamPosition = GetRoamingPosition();
            }
        }

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }
    }
    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            Debug.Log("change to roaming");
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {

            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            //else
            //{
            //    enemyPathfinding.MoveTo(roamPosition);
            //}

            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
