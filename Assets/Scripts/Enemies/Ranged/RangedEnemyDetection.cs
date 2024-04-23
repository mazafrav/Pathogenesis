using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangedEnemyDetection : MonoBehaviour
{
    public GameObject targetInRange { get; set; }
    public List<GameObject> allTargetsInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")) && !allTargetsInRange.Contains(collision.gameObject))
        {
            if (collision.gameObject.GetComponent<RangedLocomotion>() == null)
            {
                allTargetsInRange.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")) && allTargetsInRange.Contains(collision.gameObject))
        {
            allTargetsInRange.Remove(collision.gameObject);
        }
    }
}
