using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFollowRange : MonoBehaviour
{
    private List<GameObject> targets = new List<GameObject>();

    private ElectricEnemy electricEnemy;
    private ElectricLocomotion electricLocomotion;

    public GameObject chosenTarget { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        electricEnemy = GetComponentInParent<ElectricEnemy>();
        electricLocomotion = GetComponentInParent<ElectricLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0)
        {
            //if (electricEnemy.enabled) // With active AI we don want to target other electric enemies
            //{
            //    targets.RemoveAll(o => o.GetComponent<ElectricEnemy>());
            //}

            foreach(GameObject target in targets)
            {
                electricEnemy.direction = (target.transform.position - transform.position).normalized;

                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, electricEnemy.direction, 2 * electricLocomotion.FollowRange);
                for (int i = 0; i < raycastHit2D.Length; i++)
                {                                                  
                    if (raycastHit2D[i].collider.gameObject == target)
                    {
                        chosenTarget = target;                     
                        Debug.DrawRay(transform.position, electricEnemy.direction * raycastHit2D[i].distance, Color.red);
                        //if (electricEnemy.enabled && target.GetComponent<ElectricEnemy>() == null) //Electric enemies dont attack other electric enemies
                        //{
                        //}
                        //else if(!electricEnemy.enabled) // If we posses an electric enemy we can attack other electric enemies
                        //{
                        //    chosenTarget = target;
                        //    Debug.DrawRay(transform.position, electricEnemy.direction * raycastHit2D[i].distance, Color.red);
                        //}

                        break;
                    }
                    else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap") || raycastHit2D[i].collider.gameObject.CompareTag("MapElement"))
                    {
                        chosenTarget = null;
                        break;
                    }
                }
            }

        }
        else
        {
            chosenTarget = null;
        }
                  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Enemy") && collision.GetComponent<ElectricEnemy>() == null)) && !targets.Contains(collision.gameObject))
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
        }
    }

    public List<GameObject> TargetsInRange() { return targets; }
}
