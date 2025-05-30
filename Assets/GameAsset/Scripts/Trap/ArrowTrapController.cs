using System.Collections;
using UnityEngine;

public class ArrowTrapController : MonoBehaviour
{
    public BowTrap[] bowTraps;
    public float delayBetweenShots = 0.5f;  // for Sequential
    public float fireInterval = 2f;         // for AllAtOnce

    public enum FireMode { AllAtOnce, Sequential }
    public enum Direction { LeftToRight, RightToLeft }

    public FireMode fireMode = FireMode.AllAtOnce;
    public Direction fireDirection = Direction.LeftToRight;

    private Coroutine fireRoutine;

    void Start()
    {
        StartFiring();
    }

    public void StartFiring()
    {
        if (fireRoutine != null)
            StopCoroutine(fireRoutine);

        fireRoutine = StartCoroutine(fireMode == FireMode.AllAtOnce ? FireAllLoop() : FireSequentialLoop());
    }

    BowTrap[] GetOrderedBows()
    {
        if (fireDirection == Direction.LeftToRight)
            return bowTraps;
        else
        {
            BowTrap[] reversed = (BowTrap[])bowTraps.Clone();
            System.Array.Reverse(reversed);
            return reversed;
        }
    }

    IEnumerator FireAllLoop()
    {
        while (true)
        {
            foreach (var bow in GetOrderedBows())
            {
                bow.Fire();
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }

    IEnumerator FireSequentialLoop()
    {
        while (true)
        {
            foreach (var bow in GetOrderedBows())
            {
                bow.Fire();
                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
    }
}
