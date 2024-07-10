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
    protected Color newColorSprite;

    private SpriteRenderer sprite;

    // Update is called once per frame
    protected virtual void Start()
    {
        newColor = popUpText.color;
        sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null)
        {
            newColorSprite = sprite.color;
        }

    }

    protected virtual void Update()
    {
        popUpText.color = Color.Lerp(popUpText.color, newColor, lerpSpeed*Time.deltaTime);
        if (sprite != null )
        {
            sprite.color = Color.Lerp(sprite.color, newColorSprite, lerpSpeed * Time.deltaTime);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            newColor.a = 1.0f;
            if (sprite != null)
            {
                newColorSprite.a = 1.0f;
            }
        }
    }

    public void ActivateText()
    {
        newColor.a = 1.0f;
        if (sprite != null)
        {
            newColorSprite.a = 1.0f;
        }
    }

    public void DeactivateText()
    {
        newColor.a = 0f;
        if (sprite != null)
        {
            newColorSprite.a = 0f;
        }
    }
}
