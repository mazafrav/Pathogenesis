using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShock : MonoBehaviour
{
    [SerializeField] float invencibilityTime = 0.5f;
    private float currentInvecibilityTime = 0.5f;
    private bool wasPossessing = false;

    private void Start()
    {
        currentInvecibilityTime = invencibilityTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        //Debug.Log(collision.gameObject.name);

        if(collision.gameObject.CompareTag("Player") && !wasPossessing)
        {
            Destroy(collision.gameObject);
        }
        else if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy)
            {
                if(enemy.transform.parent != null) //The enemy is possessed
                {                                      
                    wasPossessing = true;
                    Debug.Log("Possessed");
                    enemy.transform.parent = null;
                    Vector3 enemyPos = enemy.transform.position;
                    enemy.DestroyEnemy();
                    GameManager.Instance.GetPlayerController().transform.position = enemyPos;
                    GameManager.Instance.GetPlayerController().EnablePlayerBody();
                    GameManager.Instance.GetPlayerController().locomotion = GameManager.Instance.GetPlayerLocomotion();
                }
                else //The enemy is not possessed
                {
                    enemy.DestroyEnemy();
                }

            }
        }

    }

    private void Update()
    {
        if(wasPossessing)
        {
            currentInvecibilityTime -= Time.deltaTime;
        }

        if(currentInvecibilityTime <= 0.0f)
        {
            currentInvecibilityTime = 0.5f;
            wasPossessing = false;
        }

    }
}
