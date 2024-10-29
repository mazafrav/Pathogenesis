using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ReceptorActivationProjectile : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 velocity;
    [SerializeField]
    private float speed = 10f;
    private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (speed > 0f)
        {
            updateVelocity();
            transform.position += velocity * Time.deltaTime;
        }

        if ((transform.position - target.transform.position).sqrMagnitude < 0.1f)
        {
            speed = 0f;
            GetComponentInChildren<VisualEffect>()?.Stop();
            Debug.Log( GetComponentInChildren<VisualEffect>());
            Destroy(gameObject, 2f);
        }
    }

    public void Initialize(GameObject target, float duration)
    {
        this.target = target;
        speed = Vector3.Distance(target.transform.position, transform.position) / duration;
        updateVelocity();
    }

    private void updateVelocity()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.z = 0;
        dir = dir.normalized;
        velocity = dir * speed;
        transform.up = dir;
    }
}
