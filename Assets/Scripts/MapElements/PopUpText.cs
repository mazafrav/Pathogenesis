using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    [SerializeField]
    protected float lerpSpeed = 1.0f;
    [SerializeField]
    protected TextMeshProUGUI popUpText;
    [SerializeField]
    protected float delay = 0.0f;
    [SerializeField]
    private bool ActivateOnlyPosessing = false;

    protected Color newColor;
    protected Color newColorControlSprite;

    protected SpriteRenderer controlSprite;

    private bool timerStart = false;

    // Update is called once per frame
    protected virtual void Start()
    {
        newColor = popUpText.color;
        controlSprite = GetComponentInChildren<SpriteRenderer>();
        if (controlSprite != null)
        {
            newColorControlSprite = controlSprite.color;
        }

    }

    protected virtual void Update()
    {
        if (timerStart) { delay -= Time.deltaTime; }

        if (delay <= 0.0f)
        {
            timerStart = false;
            popUpText.color = Color.Lerp(popUpText.color, newColor, lerpSpeed * Time.deltaTime);
            //if (controlsSprite != null)
            //{
            //    controlsSprite.color = Color.Lerp(controlsSprite.color, newColorSprite, lerpSpeed * Time.deltaTime);
            //}
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") && !ActivateOnlyPosessing) || other.CompareTag("Enemy"))
        {
            timerStart = true;
            newColor.a = 1.0f;
            if (controlSprite != null)
            {
                newColorControlSprite.a = 1.0f;
            }
        }
    }

    public void ActivateText()
    {
        timerStart = true;
        newColor.a = 1.0f;
        if (controlSprite != null)
        {
            newColorControlSprite.a = 1.0f;
        }
    }

    public void DeactivateText()
    {
        newColor.a = 0f;
        if (controlSprite != null)
        {
            newColorControlSprite.a = 0f;
        }
    }
}
