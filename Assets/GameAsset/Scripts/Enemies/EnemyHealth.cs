using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class EnemyHealth : MonoBehaviour, IPoolable
{
    [Header("Stats")]
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private EnemyType enemyType;

    [Header("Pooling")]
    [SerializeField] private GameObject enemyPrefabReference;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickupSpawner>().DropItems();

            if (GameModeManager.Instance.CurrentMode == GameMode.MainGame)
            {
                QuestManager.Instance.RegisterEnemyKilled(enemyType);
                Destroy(gameObject);
            }
            else if (GameModeManager.Instance.CurrentMode == GameMode.Survival)
            {
                GameEvents.RaiseEnemyKilledSurvival();
                ObjectPoolManager.Instance.ReturnToPool(enemyPrefabReference, this.gameObject);
            }
        }
    }
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => startingHealth;

    public void OnTakenFromPool()
    {
        currentHealth = startingHealth;
        knockback?.ResetKnockback();
        flash?.ResetFlash();
    }

    public void OnReturnedToPool()
    {
        // need clean before back to pool, put in here
        knockback?.ResetKnockback();
        flash?.ResetFlash();
    }


    //private void Die()
    //{
    //    QuestManager.Instance.RegisterEnemyKilled(enemyType);
    //    Destroy(gameObject);
    //}
}
