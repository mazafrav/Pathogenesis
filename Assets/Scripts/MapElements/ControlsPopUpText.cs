using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsPopUpText : PopUpText
{
    [SerializeField]
    private string xboxButton;

    [SerializeField]
    private string dualshockButton;

    [SerializeField]
    private string keyboardMouseButton;

    [SerializeField]
    bool showImage = false;

    GameManager gameManager;

    // Update is called once per frame
    protected override void Start()
    {
        base.Start();

        gameManager = GameManager.Instance;
    }

    protected override void Update()
    {
        base.Update();


        if (gameManager.IsThereAGamepadConnected)
        {
            
            if(gameManager.gamepadType == GameManager.GamepadType.Dualshock)
            {
                popUpText.text = dualshockButton;

                if (delay <= 0.0f)
                {
                    if (controlSprite != null)
                    {
                        controlSprite.enabled = true;
                        controlSprite.color = Color.Lerp(controlSprite.color, newColorControlSprite, lerpSpeed * Time.deltaTime);
                    }
                }
            }
            else if(gameManager.gamepadType == GameManager.GamepadType.XboxController)
            {
                popUpText.text = xboxButton;

                if (delay <= 0.0f)
                {
                    if (controlSprite != null)
                    {
                        controlSprite.enabled = true;
                        controlSprite.color = Color.Lerp(controlSprite.color, newColorControlSprite, lerpSpeed * Time.deltaTime);
                    }
                }
            }
        }
        else
        {
            popUpText.text = keyboardMouseButton;
            if (controlSprite != null)
            {
                if (!showImage)
                {
                    controlSprite.enabled = false;
                }
                else
                {
                    controlSprite.enabled = true;
                    controlSprite.color = Color.Lerp(controlSprite.color, newColorControlSprite, lerpSpeed * Time.deltaTime);
                }


            }
        }
    }

   
}
