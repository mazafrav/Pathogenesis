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

    // Start is called before the first frame update
    private void Start()
    {
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
    }

    public void ElectroShock()
    {
        activatableInterface.Activate();
        GetComponentInParent<Animator>().Play("ElectroReceptorDeactAnim");
    }
}
