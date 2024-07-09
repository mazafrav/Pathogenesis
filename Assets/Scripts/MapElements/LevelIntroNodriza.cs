using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelIntroNodriza : MonoBehaviour
{
    [SerializeField] PopUpText text;
    [SerializeField] float TimeToActivateText = 7f;
    [SerializeField] float TimeToLoadNextScene = 5f;
    
    private Animator animator;
    private bool doOnce = true;
    private bool pressedX = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pressedX )
        {
            TimeToLoadNextScene -= Time.deltaTime;
            if ( TimeToLoadNextScene < 0 )
            {
                GameManager.Instance.GetLevelLoader().StartLoadingLevel(1);
            }
        } 
        else
        {
            TimeToActivateText -= Time.deltaTime;
        }

        if (TimeToActivateText < 0 )
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
}
