using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    private string deflectEventName = "Crystalline Deflect";

    private string deflectEventPath;

    private HostLocomotion locomotion;

    private FMOD.Studio.EventInstance deflectEventInstance;

    // Start is called before the first frame update
    void Start()
    {
        locomotion = GetComponentInParent<HostLocomotion>();
        deflectEventPath = locomotion.locomotionEventNames.GenericEventsPath + deflectEventName;
        deflectEventInstance = FMODUnity.RuntimeManager.CreateInstance(deflectEventPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDeflectClip()
    {
         deflectEventInstance.start();
    }
}
