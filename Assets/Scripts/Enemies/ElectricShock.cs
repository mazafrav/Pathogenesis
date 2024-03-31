using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectricShock : MonoBehaviour
{
    [SerializeField]
    ElectricShockRange range;
    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);
    protected bool interacted = false;
    private DamageControl damageControl;

    private void Start()
    {
        damageControl = GetComponentInParent<DamageControl>();
    }

    private void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            if (o.gameObject == range.personInRange)
            {
               Debug.Log(o.gameObject.name);
                damageControl.Damage(o);
            }   
            else if (o.gameObject.GetComponent<ElectroReceptor>() != null)
            {
                o.gameObject.GetComponent<ElectroReceptor>().electroShock();
            }
        }
    }

    #region OnTriggerEnter
    //private void OnTriggerEnter2D(Collider2D collision)
    //{     
    //    Debug.Log(collision.gameObject.name);

    //    if(collision.gameObject.CompareTag("Player") && !wasPossessing)
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //    else if(collision.GetComponent<Enemy>() != null)
    //    {
    //        Enemy enemy = collision.GetComponent<Enemy>();

    //        if (enemy)
    //        {
    //            if(enemy.transform.parent != null) //The enemy is possessed
    //            {                                      
    //                wasPossessing = true;
    //                Debug.Log("Possessed");
    //                enemy.transform.parent = null;
    //                Vector3 enemyPos = enemy.transform.position;
    //                enemy.DestroyEnemy();
    //                GameManager.Instance.GetPlayerController().transform.position = enemyPos;
    //                GameManager.Instance.GetPlayerController().EnablePlayerBody();
    //                GameManager.Instance.GetPlayerController().locomotion = GameManager.Instance.GetPlayerLocomotion();
    //            }
    //            else //The enemy is not possessed
    //            {
    //                enemy.DestroyEnemy();
    //            }

    //        }
    //    }

    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && !wasPossessing)
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //    else if (collision.GetComponent<Enemy>() != null)
    //    {
    //        Enemy enemy = collision.GetComponent<Enemy>();

    //        if (enemy)
    //        {
    //            if (enemy.transform.parent != null) //The enemy is possessed
    //            {
    //                wasPossessing = true;
    //                Debug.Log("Possessed");
    //                enemy.transform.parent = null;
    //                Vector3 enemyPos = enemy.transform.position;
    //                enemy.DestroyEnemy();
    //                GameManager.Instance.GetPlayerController().transform.position = enemyPos;
    //                GameManager.Instance.GetPlayerController().EnablePlayerBody();
    //                GameManager.Instance.GetPlayerController().locomotion = GameManager.Instance.GetPlayerLocomotion();
    //            }
    //            else //The enemy is not possessed
    //            {
    //                enemy.DestroyEnemy();
    //            }

    //        }
    //    }
    //}

    //private void Update()
    //{
    //    if(wasPossessing)
    //    {
    //        currentInvecibilityTime -= Time.deltaTime;
    //    }

    //    if(currentInvecibilityTime <= 0.0f)
    //    {
    //        currentInvecibilityTime = 0.5f;
    //        wasPossessing = false;
    //    }

    //}
    #endregion OnTriggerEnter
}
