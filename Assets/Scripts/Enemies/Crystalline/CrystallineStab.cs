using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    float stabRayDistance = 1.6f;
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
                if (o.gameObject.GetComponent<KineticReceptor>() != null)
                {
                    o.gameObject.GetComponent<KineticReceptor>().Stabbed();
                }
                else
                {
                    damageControl.Damage(o);
                }
            }
        }
    }

    public bool CanStab()
    {
        Vector2 rayDirection = (endPos.position - startPos.position).normalized;       
        List<RaycastHit2D> raycastHit2D = Physics2D.RaycastAll(startPos.position, rayDirection, stabRayDistance).ToList();

        //If we find a kinetic receptor we can stab
        RaycastHit2D kineticReceptor = raycastHit2D.Find(obj => obj.collider.gameObject.GetComponent<KineticReceptor>() != null);
        if(kineticReceptor)
        {
            //endPos = kineticReceptor.collider.transform;
            return true;
        }

        //We only leave TileMap elements or moving blocks
        raycastHit2D.RemoveAll(obj => !obj.collider.gameObject.CompareTag("TileMap") && !(obj.collider is IActivatableElement));

        Debug.DrawRay(startPos.position, rayDirection * stabRayDistance, Color.red);

        //if we dont have TileMap elements or moving blocks we can stab
        return raycastHit2D.Count <= 0;
    }
}
