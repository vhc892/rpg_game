using UnityEngine;

public class TrapMover : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAtPoint = 0.5f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private Vector3 targetPosition;
    private bool isWaiting = false;

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("TrapMover: Missing pointA or pointB.");
            enabled = false;
            return;
        }
        this.transform.position = pointA.position;
        targetPosition = pointB.position;
    }

    private void Update()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(SwitchTargetAfterDelay());
        }
    }

    private System.Collections.IEnumerator SwitchTargetAfterDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        isWaiting = false;

        targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if (!other.isTrigger && player)
        {
            if(player)
            {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            }
        }
    }
}
