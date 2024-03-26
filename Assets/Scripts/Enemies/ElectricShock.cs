using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy)
            {
                if(enemy.transform.parent != null) //The enemy is possessed
                {
                    Debug.Log("Possessed");
                    enemy.transform.parent = null;
                    Vector3 enemyPos = enemy.transform.position;
                    enemy.DestroyEnemy();
                    GameManager.Instance.GetPlayer().transform.position = enemyPos;
                    GameManager.Instance.GetPlayer().EnablePlayerBody();
                    GameManager.Instance.GetPlayer().locomotion = GameManager.Instance.GetPlayerLocomotion();
                }
                else //The enemy is not possessed
                {
                    enemy.DestroyEnemy();
                }

            }
        }

    }
}
