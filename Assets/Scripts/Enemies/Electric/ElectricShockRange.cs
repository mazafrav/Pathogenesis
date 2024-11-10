using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShockRange : MonoBehaviour
{

    public GameObject personInRange {  get; private set; }
    private CrystalineEnemy crystalineEnemy;
    private bool cancellingFlee;
    private float cancelFleeTimer = 0f;

    private void Start()
    {
        crystalineEnemy = GetComponentInParent<CrystalineEnemy>();
    }

    private void Update()
    {
        if (cancellingFlee)
        {
            cancelFleeTimer = Mathf.Max(cancelFleeTimer - Time.deltaTime, 0f);

            if (cancelFleeTimer <= 0f)
            {
                personInRange = null;
                cancelFleeTimer = 0f;
                cancellingFlee = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(personInRange == null && collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            if (cancelFleeTimer > 0f)
            {
                cancelFleeTimer = 0f;
                cancellingFlee = false;
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
                cancellingFlee = true;
                cancelFleeTimer = crystalineEnemy.timeToCancelFlee;
            }
        }
    }
}
