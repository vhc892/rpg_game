using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Helper;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDeath { get; private set; }
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private TMP_Text healthText;


    public Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }
    private void Start()
    {
        isDeath = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if(enemy && other.gameObject.GetComponent<Goblin>() == null)
        {
            TakeDamage(1, other.transform);
        }
    }
    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        ScreenShakeManager.Instance.ShakeScreen();
        if (!canTakeDamage) { return; }
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }
    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0 && !isDeath)
        {
            isDeath=true;
            currentHealth = 0;
            UpdateHealthSlider();
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            if (GameModeManager.Instance.CurrentMode == GameMode.MainGame)
            {
                GetComponent<Animator>().SetTrigger(DEATH_HASH);
                StartCoroutine(DeathLoadSceneRoutine());
                Debug.Log("Player Death - MainGame");
            }
            else if (GameModeManager.Instance.CurrentMode == GameMode.Survival)
            {
                Debug.Log("Player Death - Survival");
                StartCoroutine(ShowSurvivalDeathUIRoutine());
            }
        }
    }
    private IEnumerator ShowSurvivalDeathUIRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (SurvivalWaveManager.Instance != null && SurvivalWaveManager.Instance.playerDeathUI != null)
        {
            SurvivalWaveManager.Instance.playerDeathUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.LoadLevel(0);
        Respawn();
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    private void UpdateHealthSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void Respawn()
    {
        ApplyHealthState(maxHealth);
    }

    private void ApplyHealthState(int healthValue)
    {
        currentHealth = Mathf.Clamp(healthValue, 0, maxHealth);
        isDeath = false;
        canTakeDamage = true;
        UpdateHealthSlider();

        Animator animator = GetComponent<Animator>();
        animator.ResetTrigger(DEATH_HASH);
        animator.Play("Idle");
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount; 
        UpdateHealthSlider();
    }
    public void Heal(int amount)
    {
        if (isDeath) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthSlider();
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int value)
    {
        ApplyHealthState(value);
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        UpdateHealthSlider();
    }

}
