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
            }
            else if(gameManager.gamepadType == GameManager.GamepadType.XboxController)
            {
                popUpText.text = xboxButton;
            }
        }
        else
        {
            popUpText.text = keyboardMouseButton;
        }
    }

   
}
