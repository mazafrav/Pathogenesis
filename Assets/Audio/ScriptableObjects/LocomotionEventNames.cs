using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LocomotionEventNames", fileName = "LocomotionEventNames")]
public class LocomotionEventNames : ScriptableObject
{
    public string GenericEventsPath = "event:/SFX/Enemies/";
    public string JumpEventName;
    public string LandEventName;
    public string AttackEventName;

}
