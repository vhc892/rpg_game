using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goinCoin, healthGlobe, staminaGlobe;

    public void DropItems()
    {
        Transform parentTransform = transform.parent; 
        int randomNum = Random.Range(1, 4);

        if (randomNum == 1)
        {
            GameObject item = Instantiate(staminaGlobe, transform.position, Quaternion.identity, parentTransform);
        }
        else if (randomNum == 2)
        {
            GameObject item = Instantiate(healthGlobe, transform.position, Quaternion.identity, parentTransform);
        }
        else if (randomNum == 3)
        {
            int RandomAmountOfGold = Random.Range(1, 3);
            for (int i = 0; i < RandomAmountOfGold; i++)
            {
                GameObject coin = Instantiate(goinCoin, transform.position, Quaternion.identity, parentTransform);
            }
        }
    }
}
