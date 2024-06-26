using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    public GameObject player;
    public DamageControl damageControl;
    public GameObject owner;
    [SerializeField]
    public ParticleSystem BulletVFX;
    [SerializeField]
    private AudioClip bulletHitClip;

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
        if(owner == collision.gameObject) return;

        if (collision.gameObject.GetComponent<Reflect>() != null)
        {
            Vector2 rot = Vector2.Reflect(transform.up, collision.gameObject.transform.right);
            transform.up = rot;
            HostLocomotion ownerLocomotion = collision.gameObject.GetComponentInParent<HostLocomotion>();
            if(ownerLocomotion != null)
            {
                owner = ownerLocomotion.gameObject;
            }
            else
            {
                owner = collision.gameObject;
            }

            return;
        }

        //Debug.Log("Mi pitote choc� con: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy")) //Damage an enemy
        {
            collision.GetComponent<DamageControl>().Damage(collision);
            DestroyBullet();
        }
        else if (collision.gameObject.CompareTag("Player")) //Damage a player
        {
            collision.GetComponentInParent<DamageControl>().Damage(collision);
            DestroyBullet();
        }
        else if (collision.gameObject.CompareTag("TileMap"))
        {
            DestroyBullet();
        }
        else if (collision.gameObject.CompareTag("MapElement"))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        ParticleSystem bulletVFX = Instantiate(BulletVFX, this.gameObject.transform.position, Quaternion.identity);
        bulletVFX.Play();
        HostLocomotion ownerLocomotion = owner.GetComponentInParent<HostLocomotion>();
        if(ownerLocomotion != null)
        {
            ownerLocomotion.GetOneShotSource().PlayOneShot(bulletHitClip);
        }
        Destroy(this.gameObject);
    }

}
