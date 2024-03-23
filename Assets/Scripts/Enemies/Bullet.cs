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
            // Meter lógica de dañar al jugador aquí

            Debug.Log("TE HICE MUCHO DAÑO");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
