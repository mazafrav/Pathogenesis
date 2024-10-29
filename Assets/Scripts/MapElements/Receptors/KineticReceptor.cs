using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticReceptor : MonoBehaviour
{
    [SerializeField]
    public GameObject activatableElement;
    [SerializeField]
    public float timeToDeactivate = 0f;
    [SerializeField]
    public ParticleSystem destructionVFX;
    private IActivatableElement activatableInterface;

    [SerializeField]
    private ReceptorActivationProjectile activationProjectilePrefab; 
    [SerializeField]
    private float timeToActivate = 1.5f;

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private FMODUnity.StudioEventEmitter emitter;
    [SerializeField] private float pitch = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void Stabbed()
    {
        ReceptorActivationProjectile activationProjectile = Instantiate(activationProjectilePrefab, transform.position, Quaternion.identity) as ReceptorActivationProjectile;
        activationProjectile.Initialize(activatableElement, timeToActivate);
        StartCoroutine(activateBlock(activatableElement));

        ParticleSystem bulletVFX = Instantiate(destructionVFX, this.gameObject.transform.position, Quaternion.identity);
        bulletVFX.Play();

        emitter.Play();
        emitter.EventInstance.getPitch(out float originalPitch);
        emitter.EventInstance.setPitch(originalPitch + pitch);

        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
        emitter.EventDescription.getLength(out int length);
        Destroy(this.gameObject, 2f);
    }

    private IEnumerator activateBlock(GameObject target)
    {
        yield return new WaitForSeconds(timeToActivate);
        target.GetComponent<IActivatableElement>()?.Activate();
    }
}
