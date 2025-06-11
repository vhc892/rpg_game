using System.Collections;
using UnityEngine;
using Cinemachine;

public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    private CinemachineImpulseSource source;
    private bool isShaking = false;

    [SerializeField]
    private float shakeCooldown = 0.7f;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeScreen()
    {
        if (!isShaking)
        {
            source.GenerateImpulse();
            StartCoroutine(ShakeCooldownRoutine());
        }
    }
    private IEnumerator ShakeCooldownRoutine()
    {
        isShaking = true;
        yield return new WaitForSeconds(shakeCooldown);
        isShaking = false;
    }
}
