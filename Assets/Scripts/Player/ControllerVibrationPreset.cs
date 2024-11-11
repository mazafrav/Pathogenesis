using UnityEngine;

[CreateAssetMenu(fileName = "ControllerVibrationPreset", menuName = "ScriptableObjects/ControllerVibrationPreset")]
public class ControllerVibrationPreset : ScriptableObject
{
    public AnimationCurve leftCurve = null;
    public AnimationCurve rightCurve = null;
    public bool isSimetric = true;
    public float duration = 1f;

}
