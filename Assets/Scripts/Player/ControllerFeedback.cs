using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerFeedback : MonoBehaviour
{

    [SerializeField]
    private ControllerVibrationPreset onVirusDeathPreset;
    [SerializeField]
    private ControllerVibrationPreset onHostDeathPreset;
    [SerializeField]
    private ControllerVibrationPreset onReceptorTriggerPreset;
    [SerializeField]
    private ControllerVibrationPreset onPosessionPreset;
    [SerializeField]
    private ControllerVibrationPreset onPhotonicAttackPreset;
    [SerializeField]
    private ControllerVibrationPreset onGrappleThrowPreset;
    [SerializeField]
    private ControllerVibrationPreset onGrappleHitPreset;
    [SerializeField]
    private ControllerVibrationPreset preset;
    private bool isRunning = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        GameManager.Instance.onPlayerSet += BindEvents;
        yield return new WaitForSeconds(1);
        BindEvents();
    }

    void OnDestroy()
    {
        if (GameManager.Instance.GetPlayer() && GameManager.Instance.GetPlayerController())
        {
            GameManager.Instance.GetPlayerController().onVirusDeath -= OnVirusDeath;
            GameManager.Instance.GetPlayerController().onHostDeath -= OnHostDeath;
            GameManager.Instance.GetPlayerController().onPossession -= OnPossession;
        }

        if (GameManager.Instance.IsThereAGamepadConnected)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        GameManager.Instance.onPlayerSet -= BindEvents;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) return;
        if (GameManager.Instance.IsThereAGamepadConnected &&
                Gamepad.current.rightShoulder.isPressed &&
                Gamepad.current.leftShoulder.isPressed)
        {
            Vibrate(preset);
        }
    }

    private void BindEvents()
    {
        if (GameManager.Instance.GetPlayer() && GameManager.Instance.GetPlayerController())
        {
            GameManager.Instance.GetPlayerController().onVirusDeath += OnVirusDeath;
            GameManager.Instance.GetPlayerController().onHostDeath += OnHostDeath;
            GameManager.Instance.GetPlayerController().onPossession += OnPossession;
        }
        ReceptorBase[] receptors = FindObjectsOfType<ReceptorBase>();
        foreach (ReceptorBase receptor in receptors)
        {
            receptor.onReceptorTrigger += OnReceptorTrigger;
        }
    }

    private IEnumerator SetMotorSpeed(AnimationCurve lCurve, AnimationCurve rCurve, float duration)
    {
        isRunning = true;
        float runtime = 0f;
        while (runtime < duration)
        {
            runtime += .01f;
            Gamepad.current.SetMotorSpeeds(lCurve.Evaluate(runtime / duration), rCurve.Evaluate(runtime / duration));
            yield return new WaitForSecondsRealtime(.01f);
        }
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        isRunning = false;
    }

    private IEnumerator SetMotorSpeed(AnimationCurve curve, float duration)
    {
        isRunning = true;
        float runtime = 0f;
        while (runtime < duration)
        {
            runtime += .01f;
            float value = curve.Evaluate(runtime / duration);
            Gamepad.current.SetMotorSpeeds(value, value);
            yield return new WaitForSecondsRealtime(.01f);
        }
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        isRunning = false;
    }

    private void Vibrate(ControllerVibrationPreset preset)
    {
        if (!GameManager.Instance.IsThereAGamepadConnected) return;
        if (preset.rightCurve == null || preset.isSimetric)
        {
            StartCoroutine(SetMotorSpeed(preset.leftCurve, preset.duration));
        }
        else
        {
            StartCoroutine(SetMotorSpeed(preset.leftCurve, preset.rightCurve, preset.duration));
        }
    }

    private void OnVirusDeath()
    {
        if (onVirusDeathPreset == null) return;
        Vibrate(onVirusDeathPreset);
    }

    private void OnHostDeath()
    {
        if (onHostDeathPreset == null) return;
        Vibrate(onHostDeathPreset);
    }

    private void OnPossession(HostLocomotion locomotion)
    {
        CrystallineLocomotion crystaline = locomotion as CrystallineLocomotion;
        if (crystaline != null)
        {
            crystaline.onGrappleHit += OnGrappleHit;
            crystaline.onGrappleThrow += OnGrappleThrow;
        }

        RangedLocomotion ranged = locomotion as RangedLocomotion;
        if (ranged != null)
        {
            ranged.onShoot += OnPhotonicAttack;
        }

        if (onPosessionPreset == null) return;
        Vibrate(onPosessionPreset);
    }

    private void OnReceptorTrigger()
    {
        if (onReceptorTriggerPreset == null) return;
        Vibrate(onReceptorTriggerPreset);
    }

    private void OnGrappleHit()
    {
        if (onGrappleHitPreset == null) return;
        Vibrate(onGrappleHitPreset);
    }

    private void OnGrappleThrow()
    {
        if (onGrappleThrowPreset == null) return;
        Vibrate(onGrappleThrowPreset);
    }

    private void OnPhotonicAttack()
    {
        if (onPhotonicAttackPreset == null) return;
        Vibrate(onPhotonicAttackPreset);
    }
}
