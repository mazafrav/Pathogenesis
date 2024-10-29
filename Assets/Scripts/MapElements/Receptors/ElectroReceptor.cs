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

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip activateClip;

    [SerializeField]
    private ReceptorActivationProjectile activationProjectilePrefab; 

    [SerializeField]
    private float timeToActivate = 1.5f;

    // Start is called before the first frame update
    private void Start()
    {
        currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch -= 0.5f;
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

    private IEnumerator activateBlock(GameObject target)
    {
        yield return new WaitForSeconds(timeToActivate);
        target.GetComponent<IActivatableElement>()?.Activate();
    }

    public void ElectroShock()
    {
        currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
        isActive = true;
        audioSource.PlayOneShot(activateClip);

        ReceptorActivationProjectile activationProjectile = Instantiate(activationProjectilePrefab, transform.position, Quaternion.identity) as ReceptorActivationProjectile;
        activationProjectile.Initialize(activatableElement, timeToActivate);
        StartCoroutine(activateBlock(activatableElement));
        
        GetComponentInParent<Animator>().Play("ElectroReceptorDeactAnim");
    }
}
