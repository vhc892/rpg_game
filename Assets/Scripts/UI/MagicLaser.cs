using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 0.22f;

    private bool isGrowing = true;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        LaserFaceMouse();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Indestructible>() & !other.isTrigger)
        {
            isGrowing = false;
        }
    }
    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }
    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePass = 0f;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePass += Time.deltaTime;
            float linearT = timePass / laserGrowTime;

            //sprite
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            //collider
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT)/2, capsuleCollider2D.offset.y);
            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
    private void LaserFaceMouse()
    {
        Vector3 mousePositon = Input.mousePosition;
        mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);
        Vector2 direction = transform.position - mousePositon;
        transform.right = -direction;
    }
}