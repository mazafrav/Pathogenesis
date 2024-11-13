using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPossesingEnabler : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectsToEnable;

    private bool doOnce = true;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (doOnce && collision.gameObject.GetComponent<ElectricEnemy>() && collision.GetComponentInParent<PlayerController>())
        {
            doOnce = false;
            foreach (GameObject objectToEnable in gameObjectsToEnable)
            {
                if (objectToEnable != null) { objectToEnable.SetActive(true); }
            }
        }
    }
}
