using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyToRespawn;
    private Transform[] wayPoints;
    [SerializeField] private GameObject[] enemyTemplates;
    [SerializeField] private float respawnDelay = 1.5f;
    private bool doOnce = true;
    private string locomotionType;
    // Start is called before the first frame update
    void Start()
    {
        wayPoints = enemyToRespawn.GetComponent<Enemy>().GetWayPoints();
        locomotionType = enemyToRespawn.GetComponent<HostLocomotion>().GetType().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyToRespawn == null && doOnce)
        {
            doOnce = false;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        int id = -1;
        switch (locomotionType)
        {
            case "RangedLocomotion":
                id = 0; break;
            case "ElectricLocomotion":
                id = 1; break;
            case "CrystallineLocomotion":
                id = 2; break;
            default:
                Debug.LogError("The type of locomotion to respawn is not valid");
                break;
        }

        if (id != -1)
        {
            GameObject enemy = Instantiate(enemyTemplates[id], transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetWayPoints(wayPoints);
            enemyToRespawn = enemy;
            doOnce = true;
        }
    }
}
