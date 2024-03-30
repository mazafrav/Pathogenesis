using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    private GameObject player;
    public DamageControl damageControl;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        damageControl = GetComponentInParent<DamageControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // photonic enemy cannot attack other phonotic enemies
        if (collision.gameObject.GetComponentInParent<RangedEnemy>() == null)
        {
            //if (collision.gameObject.CompareTag("Player"))
            //{
            //    // Meter lógica de dañar al jugador aquí

            //    Debug.Log("TE HICE MUCHO DAÑO");
            //}
            //else
            //{
            //    Destroy(this.gameObject);
            //}
            if (collision.gameObject.CompareTag("Player"))
            {
                damageControl.Damage(collision);
            }
            Destroy(this.gameObject);
        }

    }
}
