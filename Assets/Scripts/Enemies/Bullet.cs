using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private Rigidbody2D rb;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

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
            Health health = collision.gameObject.GetComponent<Health>();
            if (health)
            {
                health.DestroyOwner();
            }
            Debug.Log("TE HICE MUCHO DAÑO");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
