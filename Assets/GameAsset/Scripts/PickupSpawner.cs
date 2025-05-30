using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goinCoin, healthGlobe, staminaGlobe;
    
    public void DropItems()
    {
        int randomNum = Random.Range(1,4);

        if(randomNum == 1)
        {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum == 2)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        if(randomNum == 3)
        {
            int RandomAmountOfGold = Random.Range(1,5);
            for(int i = 0; i < RandomAmountOfGold; i++)
            {
                Instantiate(goinCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
