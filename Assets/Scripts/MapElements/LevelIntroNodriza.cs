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

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip heartBeatClip;
    [SerializeField]
    private AudioClip burstClip;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = heartBeatClip;
        audioSource.loop = true;
        StartCoroutine(PlayHeartBeatDelayed(0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pressedX)
        {
            if (audioSource.isPlaying && audioDoOnce)
            {
                audioDoOnce = false;
                audioSource.Stop();
                audioSource.PlayOneShot(burstClip);
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
            if (Gamepad.current != null &&  Gamepad.current.buttonSouth.isPressed)
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
        audioSource.Play();
    }
}
