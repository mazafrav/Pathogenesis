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
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip activateClip;

    // Start is called before the first frame update
    private void Start()
    {
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        audioSource = GetComponent<AudioSource>();

    }

    public void Stabbed()
    {
        activatableInterface.Activate();
        ParticleSystem bulletVFX = Instantiate(destructionVFX, this.gameObject.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(activateClip);
        bulletVFX.Play();
        Destroy(this.gameObject);
    }
}
