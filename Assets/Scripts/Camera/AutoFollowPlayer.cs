using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameManager.Instance.GetPlayer().GetComponentInChildren<Rigidbody2D>().transform;
    }
}
