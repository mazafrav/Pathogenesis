using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{
    [SerializeField]
    public static bool isCutscenePlaying = false;
    [SerializeField] PlayableDirector director;

    private void Awake()
    {
        director.stopped += DirectorStopped;
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
}
