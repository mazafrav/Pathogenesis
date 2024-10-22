using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroReceptor : MonoBehaviour
{
    [SerializeField]
    public GameObject activatableElement;
    private IActivatableElement activatableInterface;
    [SerializeField]
    public float timeToDeactivate = 0f;

    [SerializeField]
    private float timeToBeActivatedAgain = 0.4f;

    private float currentTimeToBeActivatedAgain = 0.1f;
    public bool isActive { get; private set; } = false;

    private FMODUnity.StudioEventEmitter emitter;
    [SerializeField] private float pitch = -0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {
        if (isActive)
        {
            currentTimeToBeActivatedAgain -= Time.deltaTime;
            if (currentTimeToBeActivatedAgain <= 0f)
            {
                isActive = false;
                currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
            }
        }
    }

    public void ElectroShock()
    {
        currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
        isActive = true;
        activatableInterface.Activate();
        emitter.Play();
        emitter.EventInstance.getPitch(out float originalPitch);
        emitter.EventInstance.setPitch(originalPitch + pitch);
        GetComponentInParent<Animator>().Play("ElectroReceptorDeactAnim");
    }
}
