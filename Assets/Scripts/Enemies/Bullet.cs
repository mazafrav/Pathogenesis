using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Efecto de impactar?

        if (collision.gameObject.CompareTag("Player"))
        {
            // Meter l�gica de da�ar al jugador aqu�

            Debug.Log("TE HICE MUCHO DA�O");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
