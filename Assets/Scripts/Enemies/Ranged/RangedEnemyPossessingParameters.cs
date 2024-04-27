using UnityEngine;

[CreateAssetMenu(fileName = "RangedEnemyPossessingParameters", menuName = "ScriptableObjects/RangedPossessingParameters", order = 1)]
public class RangedEnemyPossessingParameters : PossessingParameters
{
    public float shootCooldown;
    public float windUp;
}
