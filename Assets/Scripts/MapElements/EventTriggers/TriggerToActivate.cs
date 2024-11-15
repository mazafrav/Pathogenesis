using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToActivate : MonoBehaviour
{
    [SerializeField]
    public GameObject[] itemsToActivate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()))
        {

            foreach (GameObject element in itemsToActivate)
            {
                if (element != null) { element.SetActive(true); };
            }
        }
    }
}
