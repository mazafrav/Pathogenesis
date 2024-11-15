using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolvedPuzzleManager : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter emitter;
    private string parameterName = "SolvedPuzzle";

    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void PlaySound(int soundId)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, soundId);
        emitter.Play();
    }
}
