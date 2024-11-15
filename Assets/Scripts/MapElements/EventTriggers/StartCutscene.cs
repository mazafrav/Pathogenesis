using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class StartCutscene : MonoBehaviour
{
    [SerializeField]
    public static bool isCutscenePlaying = false;
    [SerializeField] PlayableDirector director;
    [SerializeField]
    private VisualEffect absortionRangeVfx;
    [SerializeField]
    private VisualEffect absortionRangeVfx2;

    [SerializeField] private SensitiveTile sensitiveTile;

    [SerializeField]
    private GameObject virus;
    [SerializeField]
    private GameObject evilVirus;

    private void Awake()
    {
        director.stopped += DirectorStopped;
    }

    private void Update()
    {
        absortionRangeVfx.SetVector3("Direction", (virus.transform.position - evilVirus.transform.position).normalized);
        absortionRangeVfx2.SetVector3("Direction", (evilVirus.transform.position - virus.transform.position).normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //GameManager.Instance.SetMusicSelectionIndex(6);
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Menu, 1f);
            director.Play();
        }
    }

    private void DirectorStopped(PlayableDirector obj)
    {
        //GameManager.Instance.GetLevelLoader().StartLoadingLevel(0);
    }

    public void StartVFX()
    {
        absortionRangeVfx.SetVector3("Direction", (virus.transform.position - evilVirus.transform.position).normalized);
        absortionRangeVfx.Play();
        //TODO: QUITAR CONTROLES
        //GameManager.Instance.SetMusicSelectionIndex(7);
        GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Epic, 1f);
        GameManager.Instance.GetPlayerController().GetPlayerIAs().Disable();
    }

    public void StartVFX2()
    {
        absortionRangeVfx2.SetVector3("Direction", (evilVirus.transform.position - virus.transform.position).normalized);
        absortionRangeVfx2.Play();
    }

    public void MoveBlock()
    {
        sensitiveTile.TriggerTargets();
        GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Epic, 0.5f);
    }

    public void EndCutscene()
    {
        GameManager.Instance.GetPlayerController().GetPlayerIAs().Enable();
        //GameManager.Instance.SetMusicSelectionIndex(0);
        GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Epic, 0f);
        GameManager.Instance.GetLevelLoader().StartLoadingLevel(0);
    }
}
