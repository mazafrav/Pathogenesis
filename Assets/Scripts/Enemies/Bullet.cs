using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    public GameObject player;
    public DamageControl damageControl;
    // Start is called before the first frame update
    void Start()
    {
        damageControl = GetComponentInParent<DamageControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= 20 || transform.position.y <= -20
            || transform.position.x >= 30 || transform.position.x <= -30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Mi pitote chocó con: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy")) //Damage an enemy
        {
            collision.GetComponent<DamageControl>().Damage(collision);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Player")) //Damage a player
        {
            collision.GetComponentInParent<DamageControl>().Damage(collision);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("TileMap"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("MapElement"))
        {
            Destroy(this.gameObject);
        }

    }
}
