using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicDetection : MonoBehaviour
{
    private CrystalineEnemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<CrystalineEnemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            enemy.closeTargets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            enemy.closeTargets.Remove(collision.gameObject);
            if (collision.gameObject == enemy.currentRangedTarget)
            {
                enemy.currentRangedTarget = null;
            }
        }
    }
}
