using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyDetection : MonoBehaviour
{
    public GameObject personInRange { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")))
        {
            personInRange = collision.gameObject;
            Debug.Log("Detectando jugador!!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            personInRange = null;
            Debug.Log("Aayy dejé de detectar al jugador :(");
        }
    }
}
