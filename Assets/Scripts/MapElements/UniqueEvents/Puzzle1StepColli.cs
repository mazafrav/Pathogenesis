using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1StepColli : MonoBehaviour
{
    [SerializeField] int soundToPlay = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()))
        {
            GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
            GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
