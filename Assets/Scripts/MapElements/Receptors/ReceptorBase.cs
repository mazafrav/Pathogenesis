using System.Collections;
using UnityEngine;

public class ReceptorBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] targetElements;

    [SerializeField]
    protected ReceptorActivationProjectile triggerVFX;
    [SerializeField]
    protected float triggerDelay = 0.5f;
    protected FMODUnity.StudioEventEmitter emitter;
    [SerializeField]
    protected float pitch = 0.5f;

    protected void Start()
    {
        foreach (var element in targetElements)
        {
            IActivatableElement activatableInterface = element.GetComponent<IActivatableElement>();
            if(activatableInterface == null)
            {
                throw new System.Exception("Object does not implement IActivatableElement");
            }
        }

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    protected void Update()
    {
        return;
    }

    public void TriggerTargets()
    {
        foreach (var element in targetElements)
        {
            ReceptorActivationProjectile triggerVFXInst = Instantiate(triggerVFX, transform.position, Quaternion.identity) as ReceptorActivationProjectile;
            triggerVFXInst.Initialize(element, triggerDelay);
            StartCoroutine(TriggerTargetDelayed(element, triggerDelay));
        }
    }

    protected IEnumerator TriggerTargetDelayed(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.GetComponent<IActivatableElement>()?.Activate();
    }

    protected void EmitSFX()
    {
        emitter.Play();
        emitter.EventInstance.getPitch(out float originalPitch);
        emitter.EventInstance.setPitch(originalPitch + pitch);
    }
}
