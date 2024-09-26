using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitchManagement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera possesCamera;

    public void setNewFollow(Transform target)
    {
        mainCamera.Follow = target;
        possesCamera.Follow = target;
    }

    public void StartPossessionEffect(float possesDuration)
    {
        SetPossesCamera();
        possesCamera.GetComponent<CameraShake>().ShakeCamera(possesDuration*2);
        StartCoroutine(PossessTimer(possesDuration));
    }

    private IEnumerator PossessTimer(float possesDuration)
    {
        yield return new WaitForSeconds(possesDuration);
        SetMainCamera();
    }

    public void SetMainCamera()
    {
        mainCamera.Priority = 10;
        possesCamera.Priority = 0;
    }

    public void SetPossesCamera()
    {
        mainCamera.Priority = 0;
        possesCamera.Priority = 10;
    }
}
