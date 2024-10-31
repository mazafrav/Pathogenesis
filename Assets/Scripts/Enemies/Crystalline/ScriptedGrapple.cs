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

            //SpriteRenderer graphics = crystallineLocomotion.GetComponentInChildren<SpriteRenderer>();
            //float angle = Vector3.Angle(transform.up, crystallineLocomotion.transform.up);
            //graphics.transform.Rotate(new Vector3(0, 0, angle), Space.Self);
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
