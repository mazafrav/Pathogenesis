using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class FotoReceptor : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FotoBullet")
        {
            activatableInterface.Open();
        }
    }
}
