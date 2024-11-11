using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelIntroNodriza : MonoBehaviour
{
    [SerializeField] PopUpText text;
    [SerializeField] float TimeToActivateText = 7f;
    [SerializeField] float TimeToLoadNextScene = 5f;
    
    private Animator animator;
    private bool doOnce = true;
    private bool audioDoOnce = true;
    private bool pressedX = false;

    private FMODUnity.StudioEventEmitter emitter;
    [SerializeField]
    private string burstEventPath = "event:/SFX/Cinematics/Intro Burst Sequence";
    private FMOD.Studio.EventInstance burstEventInstance;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        burstEventInstance = FMODUnity.RuntimeManager.CreateInstance(burstEventPath);
        StartCoroutine(PlayHeartBeatDelayed(0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pressedX)
        {
            if (emitter.IsPlaying() && audioDoOnce)
            {
                //audioDoOnce = false;
                emitter.Stop();
                burstEventInstance.start();
            }
            TimeToLoadNextScene -= Time.deltaTime;
            if (TimeToLoadNextScene <= 0)
            {
                GameManager.Instance.GetLevelLoader().StartLoadingLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
        } 
        else
        {
            TimeToActivateText -= Time.deltaTime;
        }

        if (TimeToActivateText <= 0)
        {
            text.ActivateText();
        }
        if (doOnce)
        {
            if (Gamepad.current != null && Time.timeSinceLevelLoad >= 1 && Gamepad.current.buttonSouth.isPressed)
            {
                doOnce = false;
                PressedX();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                doOnce = false;
                PressedX();
            }
        }
    }

    void PressedX()
    {
        pressedX = true;
        text.DeactivateText();
        animator.Play("VirusBurst");
    }

    IEnumerator PlayHeartBeatDelayed (float delay)
    {
        yield return new WaitForSeconds(delay);
        emitter.Play();
    }
}
