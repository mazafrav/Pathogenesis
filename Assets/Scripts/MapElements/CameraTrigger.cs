using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using Unity.VisualScripting;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] bool ChangeOffset = false;
    [SerializeField] float CameraOffsetY;
    [SerializeField] bool ChangeDeadZone = false;
    [SerializeField] float CameraDeadZoneY;

    private float baseOffset;
    private float baseDeadZone;

    private float targetOffset;
    private float targetDeadZone;

    private CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameManager.Instance.GetCamera();
        baseOffset = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
        targetOffset = baseOffset;
        baseDeadZone = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;
        targetDeadZone = baseDeadZone;
    }

    // Update is called once per frame
    void Update()
    {
        if (ChangeDeadZone)
        {
            float lerpedDeadZone = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight, targetDeadZone, 0.1f * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = lerpedDeadZone;
        }
        if (ChangeOffset)
        {
            float lerpedOffset = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, targetOffset, 0.1f * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = lerpedOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChangeDeadZone)
            {
                targetDeadZone = CameraDeadZoneY;
            }
            if (ChangeOffset)
            {
                targetOffset = CameraOffsetY;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChangeDeadZone)
            {
                targetDeadZone = baseDeadZone;
            }
            if (ChangeOffset)
            {
                targetOffset = baseOffset;
            }
        }
    }

    /*
    private void OnDestroy()
    {
         camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = baseOffset;
         camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = baseDeadZone;
    }
    */
}
