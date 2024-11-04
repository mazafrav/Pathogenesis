using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerFeedback : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve leftCurve = null;
    [SerializeField]
    private AnimationCurve rightCurve = null;
    [SerializeField]
    private bool simetricFeedback = true;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning) return;

        if (GameManager.Instance.IsThereAGamepadConnected &&
                Gamepad.current.rightShoulder.isPressed &&
                Gamepad.current.leftShoulder.isPressed)
        {
            if(rightCurve == null || simetricFeedback)
            {
                StartCoroutine(setMotorSpeed(leftCurve));
            }
            else
            {
                StartCoroutine(setMotorSpeed(leftCurve, rightCurve));
            }
        }
    }

    private IEnumerator setMotorSpeed(AnimationCurve lCurve, AnimationCurve rCurve)
    {
        isRunning = true;
        float runtime = 0f;
        while (runtime < lCurve.keys[lCurve.keys.Length - 1].time)
        {
            runtime += .01f;
            Gamepad.current.SetMotorSpeeds(lCurve.Evaluate(runtime), rCurve.Evaluate(runtime));
            yield return new WaitForSeconds(.01f);
        }
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        isRunning = false;
    }

    private IEnumerator setMotorSpeed(AnimationCurve curve)
    {
        isRunning = true;
        float runtime = 0f;
        while (runtime < curve.keys[curve.keys.Length - 1].time)
        {
            runtime += .01f;
            float value = curve.Evaluate(runtime);
            Gamepad.current.SetMotorSpeeds(value, value);
            yield return new WaitForSeconds(.01f);
        }
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        isRunning = false;
    }
}
