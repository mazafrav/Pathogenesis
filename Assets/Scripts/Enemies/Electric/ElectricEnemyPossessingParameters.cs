using UnityEngine;

[CreateAssetMenu(fileName = "ElectricEnemyPossessingParameters", menuName = "ScriptableObjects/ElectricEnemyPossessingParameters", order = 1)]
public class ElectricEnemyPossessingParameters : PossessingParameters
{
    //public float electricFollowRange;
    public float cooldown;
    public float windUp;
    public float shockRemainingTime;
    //public float shockDuration;
}
