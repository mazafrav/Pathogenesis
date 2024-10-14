using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReceptorActivationProjectile : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Vector3 speed;
    [SerializeField]
    private float lifetime = 0.1f;
    private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;
        transform.position += speed * Time.deltaTime;
        if((transform.position - target.transform.position).sqrMagnitude < 0.1f)
        {
            target.GetComponent<IActivatableElement>().Activate();
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        speed = (target.transform.position - transform.position) / lifetime;
    }
}
