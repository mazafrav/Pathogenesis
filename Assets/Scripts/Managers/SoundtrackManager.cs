using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SoundtrackManager : MonoBehaviour
{
    public enum SoundtrackParameter
    {
        Absorption,
        Danger,
        Photegenic,
        Electric,
        Crystalline        
    }

    public float PhotonicLayerIntensity { get; set; }
    public float ElectricLayerIntensity { get; set; }
    public float CrystallineLayerIntensity { get; set; }

    [SerializeField] private string pauseSnapshotPath = "snapshot:/Pause_Menu";
    private FMOD.Studio.EventInstance snapshotInstance;

    private FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (!emitter.IsPlaying())
        {
            emitter.Play();
        }
        ResetSoundtrack();

        snapshotInstance = FMODUnity.RuntimeManager.CreateInstance(pauseSnapshotPath);
    }

    public void ChangeSoundtrackParameter(SoundtrackParameter parameter, float value)
    {
        string parameterString = parameter.ToString();

        if (parameter == SoundtrackParameter.Absorption)
        {
            emitter.EventInstance.setParameterByName(parameterString, value);
        }
        else
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterString, value);
        }
    }

    public void CompareEnemyLayer(int id, float value)
    {
        bool updated = false;
        switch(id)
        {
            case (int)SoundtrackParameter.Photegenic:
                updated = value > PhotonicLayerIntensity;
                if (updated)
                {
                    PhotonicLayerIntensity = value;
                }
                break;
            case (int)SoundtrackParameter.Electric:
                updated = value > ElectricLayerIntensity;
                if (updated)
                {
                    ElectricLayerIntensity = value;
                }
                break;
            case (int)SoundtrackParameter.Crystalline:
                updated = value > CrystallineLayerIntensity;
                if (updated)
                {
                    CrystallineLayerIntensity = value;
                }
                break;
        }

        if (updated)
        {
            ChangeSoundtrackParameter((SoundtrackParameter)id, value);
        }
    }

    public void ClearEnemyLayer(int id)
    {
        switch (id)
        {
            case (int)SoundtrackParameter.Photegenic:

                PhotonicLayerIntensity = 0;
                break;
            case (int)SoundtrackParameter.Electric:

                ElectricLayerIntensity = 0;
                break;
            case (int)SoundtrackParameter.Crystalline:

                CrystallineLayerIntensity = 0;
                break;
        }

    }

    public void ResetSoundtrack()
    {
        string[] soundtrackParameterNames = System.Enum.GetNames(typeof(SoundtrackParameter));
        foreach (string name in soundtrackParameterNames)
        {
            if (name.Equals(SoundtrackParameter.Absorption.ToString()))
            {
                emitter.EventInstance.setParameterByName(name, 0);
            }
            else
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, 0);
            }
        }
    }

    private void OnEnable()
    {
        if (emitter == null)
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }

        if (!emitter.IsPlaying())
        {
            emitter.Play();
        }
    }

    private void OnDisable()
    {
        if (emitter == null)
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }
        if (emitter.IsPlaying())
        {
            emitter.Stop();
        }
    }

    public void ApplyPauseSnapshot()
    {
        snapshotInstance.start();
    }

    public void StopPausepSnapshot()
    {
        snapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
