using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShockRange : MonoBehaviour
{

    public GameObject personInRange {  get; private set; }
    private CrystalineEnemy crystalineEnemy;
    private bool cancellingAggro;
    private float cancelAggroTimer = 0f;

    private void Start()
    {
        crystalineEnemy = GetComponentInParent<CrystalineEnemy>();
    }

    private void Update()
    {
        if (cancellingAggro)
        {
            cancelAggroTimer = Mathf.Max(cancelAggroTimer - Time.deltaTime, 0f);

            if (cancelAggroTimer <= 0f)
            {
                personInRange = null;
                cancelAggroTimer = 0f;
                cancellingAggro = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            if (cancelAggroTimer > 0f)
            {
                cancelAggroTimer = 0f;
                cancellingAggro = false;
            }
            else
            {
                personInRange = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            if (!crystalineEnemy)
            {
                personInRange = null;
            }
            else
            {
                cancellingAggro = true;
                cancelAggroTimer = crystalineEnemy.timeToCancelAggro;
            }
        }
    }
}
