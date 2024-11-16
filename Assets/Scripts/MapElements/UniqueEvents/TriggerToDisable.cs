using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToDisable : MonoBehaviour
{

    [SerializeField] public GameObject[] itemsToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()))
        {

            foreach (GameObject element in itemsToActivate)
            {
                if (element != null) { element.SetActive(false); };
            }
            GetComponent<SolvedPuzzleManager>().PlaySound(2);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
