using UnityEngine;

[CreateAssetMenu(fileName = "CrystallineEnemyPossessingParameters", menuName = "ScriptableObjects/CrystallineEnemyPossessingParameters", order = 1)]
public class CrystallineEnemyPossessingParameters : PossessingParameters
{
    public float stabRange;
    public float cooldown;
    public float windUp;  
    public float stabDuration;
}
