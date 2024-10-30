using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedGrapple : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrystallineLocomotion crystallineLocomotion = collision.gameObject.GetComponent<CrystallineLocomotion>();
        //The enemy dont have to be possessed
        if (crystallineLocomotion && crystallineLocomotion.transform.parent == null)
        {
            crystallineLocomotion.GetGrapplingHook().LaunchGrapple();
        }
    }
}
