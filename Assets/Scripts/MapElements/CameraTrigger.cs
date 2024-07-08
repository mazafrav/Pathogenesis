using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using Unity.VisualScripting;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] bool ChangeOffsetY = false;
    [SerializeField] float CameraOffsetY;
    [SerializeField] bool ChangeDeadZoneY = false;
    [SerializeField] float CameraDeadZoneY;
    [SerializeField] bool ChangeOffsetX = false;
    [SerializeField] float CameraOffsetX;
    [SerializeField] bool ChangeDeadZoneX = false;
    [SerializeField] float CameraDeadZoneX;
    [SerializeField] float speed = 0.4f;

    private float baseOffsetY;
    private float baseDeadZoneY;
    private float baseOffsetX;
    private float baseDeadZoneX;

    private float targetOffsetY;
    private float targetDeadZoneY;
    private float targetOffsetX;
    private float targetDeadZoneX;

    private CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameManager.Instance.GetCamera();
        baseOffsetY = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
        targetOffsetY = baseOffsetY;
        baseDeadZoneY = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;
        targetDeadZoneY = baseDeadZoneY;
        baseOffsetX = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
        targetOffsetX = baseOffsetX;
        baseDeadZoneX = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth;
        targetDeadZoneX = baseDeadZoneX;
    }

    // Update is called once per frame
    void Update()
    {
        if (ChangeDeadZoneY)
        {
            float lerpedDeadZone = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight, targetDeadZoneY, speed * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = lerpedDeadZone;
        }
        if (ChangeOffsetY)
        {
            float lerpedOffset = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, targetOffsetY, speed * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = lerpedOffset;
        }
        if (ChangeDeadZoneX)
        {
            float lerpedDeadZone = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth, targetDeadZoneX, speed * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = lerpedDeadZone;
        }
        if (ChangeOffsetX)
        {
            float lerpedOffset = Mathf.Lerp(camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, targetOffsetX, speed * Time.deltaTime);
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = lerpedOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChangeDeadZoneY)
            {
                targetDeadZoneY = CameraDeadZoneY;
            }
            if (ChangeOffsetY)
            {
                targetOffsetY = CameraOffsetY;
            }
            if (ChangeDeadZoneX)
            {
                targetDeadZoneX = CameraDeadZoneX;
            }
            if (ChangeOffsetX)
            {
                targetOffsetX = CameraOffsetX;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChangeDeadZoneY)
            {
                targetDeadZoneY = baseDeadZoneY;
            }
            if (ChangeOffsetY)
            {
                targetOffsetY = baseOffsetY;
            }
            if (ChangeDeadZoneX)
            {
                targetDeadZoneX = baseDeadZoneX;
            }
            if (ChangeOffsetX)
            {
                targetOffsetX = baseOffsetX;
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
