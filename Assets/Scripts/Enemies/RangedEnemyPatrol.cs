using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent.transform.localScale = 
            new Vector2(-(Mathf.Sign(transform.parent.GetComponent<Rigidbody2D>().velocity.x)), 
            transform.parent.transform.localScale.y);
    }
}
