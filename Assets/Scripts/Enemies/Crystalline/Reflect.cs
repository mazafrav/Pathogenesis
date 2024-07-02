using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    private AudioClip deflectClip;

    private HostLocomotion locomotion;

    // Start is called before the first frame update
    void Start()
    {
        locomotion = GetComponentInParent<HostLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDeflectClip()
    {
        locomotion.GetOneShotSource().PlayOneShot(deflectClip);
    }
}
