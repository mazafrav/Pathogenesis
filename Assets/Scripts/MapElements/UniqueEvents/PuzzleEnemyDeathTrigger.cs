using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEnemyDeathTrigger : MonoBehaviour
{
    [SerializeField] int soundToPlay = 0;
    [SerializeField] GameObject enemyToCheck;

    private bool doOnce = true;

    void Update()
    {
        if (doOnce && enemyToCheck == null)
        {
            GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
            doOnce = false;
        }
    }
}
