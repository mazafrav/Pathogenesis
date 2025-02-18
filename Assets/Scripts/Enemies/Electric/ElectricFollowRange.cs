using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricFollowRange : MonoBehaviour
{
    private List<GameObject> targets = new List<GameObject>(); //Targets in range which can or cant be visibles
    private List<GameObject> visibleTargets = new List<GameObject>(); //Targets in range which are visibles

    private ElectricEnemy electricEnemy;
    private ElectricLocomotion electricLocomotion;

    // Start is called before the first frame update
    void Start()
    {
        electricEnemy = GetComponentInParent<ElectricEnemy>();
        electricLocomotion = GetComponentInParent<ElectricLocomotion>();
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0)
        {           
            foreach(GameObject target in targets)
            {
                if (target == null) 
                { 
                    continue; 
                }

                Vector2 rayDirection = (target.transform.position - transform.position).normalized;
                List<RaycastHit2D> raycastHit2D = Physics2D.RaycastAll(transform.position, rayDirection, 2 * electricLocomotion.FollowRange).ToList();            

                raycastHit2D.RemoveAll(obj => !obj.collider.gameObject.CompareTag("Player") && !obj.collider.gameObject.CompareTag("Enemy") && !obj.collider.gameObject.CompareTag("TileMap") && !obj.collider.gameObject.CompareTag("MapElement"));
                //An organism is in range but we cant see him
                if (raycastHit2D.Count > 0 && (raycastHit2D[0].collider.gameObject.CompareTag("TileMap") || raycastHit2D[0].collider.gameObject.CompareTag("MapElement")))
                {
                    visibleTargets.Remove(target);
                    continue;
                }
           
                //At this point we have all visible targets in range
                electricEnemy.direction = (target.transform.position - transform.position).normalized;
             
                for (int i = 0; i < raycastHit2D.Count; i++)
                {                                                  
                    if (raycastHit2D[i].collider.gameObject == target && !visibleTargets.Contains(target))
                    {          
                        //Debug.DrawRay(transform.position, electricEnemy.direction * raycastHit2D[i].distance, Color.red);
                        visibleTargets.Add(target);                      
                    }                
                }           
            }

            if (visibleTargets.Count <= 0) 
            {
                electricEnemy.SetIsSeeingTarget(false);
            }
            else
            {
                electricEnemy.SetIsSeeingTarget(true);
            }

        }
        else
        {
            electricEnemy.SetIsSeeingTarget(false);
        }
              
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ElectricEnemy otherElectricEnemy = collision.GetComponent<ElectricEnemy>();

        if (electricEnemy.CanAttackSameSpecie && otherElectricEnemy && otherElectricEnemy.IsPossesed && !targets.Contains(collision.gameObject)) //Possessed electric enemy
        {
            targets.Add(collision.gameObject);
        } 
        else if ((collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Enemy") && collision.GetComponent<ElectricEnemy>() == null)) && !targets.Contains(collision.gameObject)) //Other enemies except electric
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ElectricEnemy otherElectricEnemy = collision.GetComponent<ElectricEnemy>();

        if (electricEnemy.CanAttackSameSpecie && otherElectricEnemy && otherElectricEnemy.IsPossesed && !targets.Contains(collision.gameObject)) //Possessed electric enemy
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
            visibleTargets.Remove(collision.gameObject);
        }
    }

    public List<GameObject> TargetsInRange() { return targets; }
    public List<GameObject> VisibleTargetsInRange() { return visibleTargets; }
}
