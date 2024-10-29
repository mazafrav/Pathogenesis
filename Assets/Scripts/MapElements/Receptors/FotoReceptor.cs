using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class FotoReceptor : MonoBehaviour
{
    [SerializeField]
    public GameObject[] activatableElement;
    [SerializeField]
    public float timeToDeactivate = 0f;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip activateClip;
    [SerializeField]
    private ReceptorActivationProjectile activationProjectilePrefab; 
    [SerializeField]
    private float timeToActivate = 1.5f;

    private FMODUnity.StudioEventEmitter emitter;
    [SerializeField] private float pitch = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var element in activatableElement)
        {
            IActivatableElement activatableInterface = element.GetComponent<IActivatableElement>();
            if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        }

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FotoBullet")
        {

            emitter.Play();
            emitter.EventInstance.getPitch(out float originalPitch);
            emitter.EventInstance.setPitch(originalPitch + pitch);

            foreach (var element in activatableElement)
            {
                ReceptorActivationProjectile activationProjectile = Instantiate(activationProjectilePrefab, transform.position, Quaternion.identity) as ReceptorActivationProjectile;
                activationProjectile.Initialize(element, timeToActivate);
                StartCoroutine(activateBlock(element));
                GetComponentInParent<Animator>().Play("FotoReceptorDeactAnim");
            }
        }
    }

    private IEnumerator activateBlock(GameObject target)
    {
        yield return new WaitForSeconds(timeToActivate);
        target.GetComponent<IActivatableElement>()?.Activate();
    }
}
