using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Unity.VisualScripting;

public class ButtonHighlightFX : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ParticleSystem[] _particleSystem;

    [SerializeField] private string hoverSFXPath = "event:/UI/Menu Button Hover";
    [SerializeField] private string selectSFXPath = "event:/UI/Menu Button Select";

    private FMOD.Studio.EventInstance hoverInstance;
    private FMOD.Studio.EventInstance selectInstance;

    private void Start()
    {
        hoverInstance = FMODUnity.RuntimeManager.CreateInstance(hoverSFXPath);
        selectInstance = FMODUnity.RuntimeManager.CreateInstance(selectSFXPath);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("ButtonHighlight");
        }

        hoverInstance.start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("ButtonAnim");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("ButtonHighlight");
        }

    }

    public void OnDeselect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("ButtonAnim");
        }
    }

    public void OnClick()
    {
        GetComponent<Animator>().Play("ButtonSelected");


        selectInstance.start();
    }
}
