using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleColliFotoBlockState : MonoBehaviour
{
    [SerializeField] int soundToPlay = 0;
    [SerializeField] PhotonicBlock block;
    [SerializeField] bool stateToCheck = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()))
        {
            if (block.IsOpened == stateToCheck)
            {
                GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
