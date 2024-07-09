using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    [SerializeField]
    public float lerpSpeed = 1.0f;
    [SerializeField]
    private TextMeshProUGUI popUpText;
    private Color newColor;

    // Update is called once per frame
    private void Start()
    {
        newColor = popUpText.color;
    }

    void Update()
    {
        popUpText.color = Color.Lerp(popUpText.color, newColor, lerpSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
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
