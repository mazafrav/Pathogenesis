using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleColli2BlockState : MonoBehaviour
{
    [SerializeField] int soundToPlay = 0;
    [SerializeField] MovingBlock block1;
    [SerializeField] bool stateToCheck1 = false;
    [SerializeField] ElectroBlock block2;
    [SerializeField] bool stateToCheck2 = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()))
        {
            if (block1.IsOpened == stateToCheck1 && block2.IsOpened == stateToCheck2)
            {
                GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
