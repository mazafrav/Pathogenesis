using UnityEngine;

public class KineticReceptor : ReceptorBase
{
    [SerializeField]
    private ParticleSystem destructionVFX;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Stabbed()
    {
        EmitSFX();
        TriggerTargets();

        ParticleSystem bulletVFX = Instantiate(destructionVFX, transform.position, Quaternion.identity);
        bulletVFX.Play();


        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
        //emitter.EventDescription.getLength(out int length);
        Destroy(gameObject, 2f);
    }
}
