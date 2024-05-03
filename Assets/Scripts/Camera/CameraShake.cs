using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();      
    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0.0f, 1.0f - (shakeTimer / shakeTimerTotal));

            }          
        }
    }


    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
        startingIntensity = intensity;
    }
}
