using UnityEngine;

[CreateAssetMenu(fileName = "ElectricEnemyPossessingParameters", menuName = "ScriptableObjects/ElectricEnemyPossessingParameters", order = 1)]
public class ElectricEnemyPossessingParameters : PossessingParameters
{
    public float electricShockRange;
    public float cooldown;
    public float windUp;
    public float shockDuration;
}
