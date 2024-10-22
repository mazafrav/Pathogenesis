using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectricShock : MonoBehaviour
{
    [SerializeField]
    private ElectricFollowRange followRange;
    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    private DamageControl damageControl;

    private FMODUnity.StudioEventEmitter emitter;

    private void Awake()
    {
        damageControl = GetComponentInParent<DamageControl>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);

        foreach (var o in collidedObjects)
        {                                                                             
            if (followRange.gameObject.activeSelf) //Attack when is controlled by AI   
            {
                List<GameObject> visibleTragets = followRange.VisibleTargetsInRange();
                for (int i = 0; visibleTragets.Count > i; i++)
                {
                    if(o.gameObject == visibleTragets[i])
                    {
                        damageControl.Damage(o);
                    }
                }
            }
            else if (!followRange.gameObject.activeSelf && o.gameObject.GetComponent<Enemy>() && o.gameObject != damageControl.gameObject) //Attack when is possessed
            {
                damageControl.Damage(o);
            }
            else if (o.gameObject.GetComponent<ElectroReceptor>() != null)
            {
                o.gameObject.GetComponent<ElectroReceptor>().ElectroShock();
            }
            else if (o.gameObject.GetComponent<CrystalBlock>() != null)
            {
                o.gameObject.GetComponent<CrystalBlock>().DestroyCrystalBlock();
            }
        }

        if (GameManager.Instance.isPaused)
        {
            emitter.Stop();
        }
        else
        {
            if (!emitter.IsPlaying())
            {
                emitter.Play();
            }
        }
    }   
}
