using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;

    public void DestroyOwner()
    {
        Destroy(objectToDestroy); 
    }
}
