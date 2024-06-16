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

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var element in activatableElement)
        {
            IActivatableElement activatableInterface = element.GetComponent<IActivatableElement>();
            if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FotoBullet")
        {
            foreach (var element in activatableElement)
            {
                element.GetComponent<IActivatableElement>().Open();
            }
        }
    }
}
