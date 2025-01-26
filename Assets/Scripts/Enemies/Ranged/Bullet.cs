using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float speed = 10.0f;
    [SerializeField]
    private ParticleSystem BulletVFX;
    [SerializeField]
    private float timeToDestroy = 10.0f;

    private DamageControl damageControl;
    private FMODUnity.StudioEventEmitter emitter;


    public GameObject Owner { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        damageControl = GetComponentInParent<DamageControl>();

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        //If the bullet doesnt collide with anything destroy it after x seconds
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Owner == collision.gameObject) return;

        if (collision.gameObject.GetComponent<Reflect>() != null)
        {
            collision.gameObject.GetComponent<Reflect>().PlayDeflectClip();
            Vector2 rot = Vector2.Reflect(transform.up, collision.gameObject.transform.right);
            transform.up = rot;
            HostLocomotion ownerLocomotion = collision.gameObject.GetComponentInParent<HostLocomotion>();
            if(ownerLocomotion != null)
            {
                Owner = ownerLocomotion.gameObject;
            }
            else
            {
                Owner = collision.gameObject;
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
            collision.GetComponentInChildren<DamageControl>().Damage(collision);
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
       
        emitter.Play();

        Destroy(this.gameObject);
    }
}
