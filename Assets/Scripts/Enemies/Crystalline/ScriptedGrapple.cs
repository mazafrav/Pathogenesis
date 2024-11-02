using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedGrapple : MonoBehaviour
{
    [SerializeField]
    private GameObject aimPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrystallineLocomotion crystallineLocomotion = collision.gameObject.GetComponent<CrystallineLocomotion>();
        //The enemy dont have to be possessed
        if (crystallineLocomotion && crystallineLocomotion.transform.parent == null)
        {
            GrapplingHook grapplingHook = crystallineLocomotion.GetGrapplingHook();
            grapplingHook.SetAimPoint(aimPoint);
            grapplingHook.LaunchGrapple();

            SpriteRenderer graphics = crystallineLocomotion.GetComponentInChildren<SpriteRenderer>();
            graphics.transform.right = aimPoint.transform.position - graphics.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CrystallineLocomotion crystallineLocomotion = collision.gameObject.GetComponent<CrystallineLocomotion>();
        //The enemy dont have to be possessed
        if (crystallineLocomotion && crystallineLocomotion.transform.parent == null)
        {
            GrapplingHook grapplingHook = crystallineLocomotion.GetGrapplingHook();         
            grapplingHook.ResetAimPoint();
        }
    }
}
