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
        Menu,
        Epic
    }

    public float PhotonicLayerIntensity { get; set; }
    public float ElectricLayerIntensity { get; set; }
    public float CrystallineLayerIntensity { get; set; }

    //[SerializeField] private string mainMusicPath = "event:/Music/Menu_End";
    //[SerializeField] private string ambientMusicPath = "event:/Music/Ambient";
    [SerializeField] private string pauseSnapshotPath = "snapshot:/Pause_Menu";

    //private FMOD.Studio.EventInstance mainMusicInstance;
    //private FMOD.Studio.EventInstance ambientMusicInstance;
    private FMOD.Studio.EventInstance snapshotInstance;

     private Bus sfxBus;
     private Bus musicBus;

    //private FMOD.Studio.EventInstance currentInstance;

    private FMODUnity.StudioEventEmitter emitter;
    private bool audioDoOnce = true;

    private void Start()
    {
        SceneManager.sceneLoaded += CheckLevelSoundtrack;

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        //mainMusicInstance = FMODUnity.RuntimeManager.CreateInstance(mainMusicPath);
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
            ChangeSoundtrackParameter(SoundtrackParameter.Menu, 1);
            if (!emitter.IsPlaying())
            {
                emitter.Play();
            }
            //emitter.Stop();
        }
        else if (SceneManager.GetActiveScene().name.Equals("LVL_0"))
        {
            //emitter.Stop();
            ChangeSoundtrackParameter(SoundtrackParameter.Menu, 0.5f);
            //mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //mainMusicInstance.release();
        }

        SetBuses();

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
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, 0);
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

    private void CheckLevelSoundtrack(Scene scene, LoadSceneMode mode)
    {
        StopAllSFX();

        if (scene.name.Equals("PauseMenu") || scene.name.Equals("SettingsMenu"))
        {
            return;
        }

        if (scene.name.Equals("LVL_0"))
        {
            //emitter.Stop();
            ChangeSoundtrackParameter(SoundtrackParameter.Menu, 0.5f);

        }
        else
        {
            if (!emitter.IsPlaying())
            {
                emitter.Play();
            }

            if (scene.name.Equals("MainMenu"))
            {
                ChangeSoundtrackParameter(SoundtrackParameter.Menu, 1);
            }
            else
            {
                ChangeSoundtrackParameter(SoundtrackParameter.Menu, 0);
            }
        }

    }

    public void SetBuses(bool debug = false)
    {
        Bank[] bankList;
        FMODUnity.RuntimeManager.StudioSystem.getBankList(out bankList);

        foreach (var bank in bankList)
        {
            Bus[] buses;
            bank.getBusList(out buses);

            foreach (var bus in buses)
            {
                bus.getPath(out string pathBus);
                if (pathBus.Contains("SFX"))
                {
                    sfxBus = bus;
                }
                else if (pathBus.Contains("Music"))
                {
                    musicBus = bus;
                }
            }

        }

        if (debug)
        {
            sfxBus.getPath(out string sfxpath);
            musicBus.getPath(out string musicpath);

            Debug.Log("SFX bus loaded from " + sfxpath);
            Debug.Log("Music bus loaded from " + musicpath);
        }

    }

    public void StopAllSFX()
    {
        sfxBus.stopAllEvents(STOP_MODE.IMMEDIATE);
    }

    public void StopAllMusic()
    {
        musicBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckLevelSoundtrack;
    }
}
