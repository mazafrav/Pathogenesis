using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticReceptor : MonoBehaviour
{
    [SerializeField]
    public GameObject activatableElement;
    [SerializeField]
    public float timeToDeactivate = 0f;
    private IActivatableElement activatableInterface;

    // Start is called before the first frame update
    private void Start()
    {
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
    }

    public void Stabbed()
    {
        activatableInterface.Open();
    }
}
