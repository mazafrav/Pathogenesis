using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    public GameObject player;
    public DamageControl damageControl;

    private bool isReflected = false;
    // Start is called before the first frame update
    void Start()
    {
        damageControl = GetComponentInParent<DamageControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        /*
        if (transform.position.y >= 20 || transform.position.y <= -20
            || transform.position.x >= 30 || transform.position.x <= -30)
        {
            Destroy(gameObject);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<RangedEnemy>() == null)
        {
            if (collision.gameObject.GetComponent<Reflect>() != null)
            {
                Debug.Log("Collided with reflect");
                transform.up = -transform.up;
                isReflected = true;
                return;
            }

            //Debug.Log("Mi pitote choc� con: " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy")) //Damage an enemy
            {
                if (collision.gameObject.GetComponent<CrystallineLocomotion>() != null && isReflected)
                {
                    isReflected = false;
                    return;
                }
                Debug.LogWarning("Rip cristalino");
                collision.GetComponent<DamageControl>().Damage(collision);
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.CompareTag("Player")) //Damage a player
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
}
