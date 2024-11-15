using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEnemyDeathAndBlock : MonoBehaviour
{
    [SerializeField] int soundToPlay = 0;
    [SerializeField] GameObject enemyToCheck;
    [SerializeField] ElectroBlock block;
    [SerializeField] bool stateToCheck = false;

    private bool doOnce = true;

    void Update()
    {
        if (doOnce && enemyToCheck == null && block.isOpened == stateToCheck)
        {
            GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
            doOnce = false;
        }
    }
}
