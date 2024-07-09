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
    protected Color newColor;

    // Update is called once per frame
    protected virtual void Start()
    {
        newColor = popUpText.color;
    }

    protected virtual void Update()
    {
        popUpText.color = Color.Lerp(popUpText.color, newColor, lerpSpeed*Time.deltaTime);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            newColor.a = 1.0f;
        }
    }

    public void ActivateText()
    {
        newColor.a = 1.0f;
    }

    public void DeactivateText()
    {
        newColor.a = 0f;
    }
}
