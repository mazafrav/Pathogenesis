using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystallineStab : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;
    [SerializeField]
    private GameObject stabGameObject;

    [SerializeField]
    private ContactFilter2D filter;
    private DamageControl damageControl;
    public bool isDamageActive;
    private Collider2D interactionCollider;

    void Start()
    {
        damageControl = GetComponentInParent<DamageControl>();
        interactionCollider = GetComponentInChildren<Collider2D>();
    }

    public void MoveStab(float alpha)
    {
        stabGameObject.transform.position = Vector2.Lerp(startPos.position, endPos.position, alpha);
    }

    private void Update()
    {
        if (interactionCollider == null || !isDamageActive) return;

        List<Collider2D> collidedObjects = new();
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            if(!transform.IsChildOf(o.transform))
            {
                damageControl.Damage(o);
            }
            else if (o.gameObject.GetComponent<KineticReceptor>() != null)
            {
                o.gameObject.GetComponent<KineticReceptor>().Stabbed();
            }
        }
    }
}
