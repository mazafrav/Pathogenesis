using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField] float lerpSpeed = .5f;
    [SerializeField] float permanenceTime = 0.5f;

    float targetAlpha = 0f;
    float eventTime;
    float lastRotation;
    SpriteRenderer sprite;

    private void Awake()
    {
        eventTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = targetAlpha;
        sprite.color = color;
        lastRotation = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= eventTime)
        {
            float newRotation = transform.rotation.z;
            targetAlpha = 0f;
            if (lastRotation != newRotation)
            {
                targetAlpha = 1f;
                StartTimer();
            } 
            lastRotation = newRotation;
        }
        float newAlpha = Mathf.Lerp(sprite.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
        Color newColor = sprite.color;
        newColor.a = newAlpha;
        sprite.color = newColor;

    }

    public void StartTimer()
    {
        eventTime = Time.time + permanenceTime;
    }
}
