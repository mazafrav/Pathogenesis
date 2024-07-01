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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            director.Play();
        }
    }

    private void DirectorStopped(PlayableDirector obj)
    {
        GameManager.Instance.GetLevelLoader().StartLoadingLevel(0);
    }

    public void StartVFX()
    {
        absortionRangeVfx.SetVector3("Direction", (virus.transform.position - evilVirus.transform.position).normalized);
        absortionRangeVfx.Play();
    }
}
