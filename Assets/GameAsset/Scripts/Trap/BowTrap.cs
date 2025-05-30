using UnityEngine;

public class BowTrap : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public Vector2 arrowDirection = Vector2.right;

    public void Fire()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity, transform.parent);

        if (arrowDirection == Vector2.left)
        {
            Vector3 scale = arrow.transform.localScale;
            scale.x *= -1;
            arrow.transform.localScale = scale;
        }

        arrow.GetComponent<Arrow>().SetDirection(arrowDirection);
    }



}
