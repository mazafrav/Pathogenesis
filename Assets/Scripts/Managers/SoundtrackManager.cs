using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundtrackManager : MonoBehaviour
{
    public enum SoundtrackParameter
    {
        Absorption,
        Danger,
        Photegenic,
        Electric,
        Crystalline,
        Epic
    }

    public float PhotonicLayerIntensity { get; set; }
    public float ElectricLayerIntensity { get; set; }
    public float CrystallineLayerIntensity { get; set; }

    [SerializeField] private string mainMusicPath = "event:/Music/Menu_End";
    //[SerializeField] private string ambientMusicPath = "event:/Music/Ambient";
    [SerializeField] private string pauseSnapshotPath = "snapshot:/Pause_Menu";

    private FMOD.Studio.EventInstance mainMusicInstance;
    //private FMOD.Studio.EventInstance ambientMusicInstance;
    private FMOD.Studio.EventInstance snapshotInstance;

    //[SerializeField] private FMOD.Studio.Bus sfxBus;
    //[SerializeField] private FMOD.Studio.Bus musicBus;

    //private FMOD.Studio.EventInstance currentInstance;

    private FMODUnity.StudioEventEmitter emitter;
    private bool audioDoOnce = true;

    private void Start()
    {
        SceneManager.sceneLoaded += CheckLevelSoundtrack;

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        mainMusicInstance = FMODUnity.RuntimeManager.CreateInstance(mainMusicPath);
        //ambientMusicInstance = FMODUnity.RuntimeManager.CreateInstance(ambientMusicPath);
        snapshotInstance = FMODUnity.RuntimeManager.CreateInstance(pauseSnapshotPath);

        //if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        //{
        //    currentInstance = mainMusicInstance;
        //}
        //else
        //{
        //    currentInstance = ambientMusicInstance;
        //}

        //currentInstance.start();

        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            mainMusicInstance.start();
            emitter.Stop();
        }
        else if (SceneManager.GetActiveScene().name.Equals("LVL_0"))
        {
            emitter.Stop();
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainMusicInstance.release();
        }

    }

    public void ChangeSoundtrackParameter(SoundtrackParameter parameter, float value)
    {
        string parameterString = parameter.ToString();

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterString, value);

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
                //ambientMusicInstance.setParameterByName(name, 0);
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
        snapshotInstance.release();
    }

    private void CheckLevelSoundtrack(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("MainMenu"))
        {
            audioDoOnce = true;

            emitter.Stop();
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainMusicInstance.release();
            mainMusicInstance.start();
        }
        else if (scene.name.Equals("LVL_0"))
        {
            emitter.Stop();
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainMusicInstance.release();
        }
        else if (audioDoOnce && scene.name.Equals("LVL_12"))
        {
            audioDoOnce = false;
            emitter.Stop();
            mainMusicInstance.start();
        }
        else
        {
            if (!emitter.IsPlaying())
            {
                mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                mainMusicInstance.release();
                emitter.Play();
            }
        }

    }

    //public void StopAllSFX()
    //{
    //    sfxBus.stopAllEvents(STOP_MODE.IMMEDIATE);
    //    //FMODUnity.RuntimeManager.StudioSystem.getBankList(out bankList);
    //}

    //public void StopAllMusic()
    //{
    //    musicBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
    //}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckLevelSoundtrack;
    }
}
