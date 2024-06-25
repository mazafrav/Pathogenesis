using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField]
    float CameraOffsetY;

    private CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameManager.Instance.GetCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //CinemachineComponentBase body = camera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
    }
}
