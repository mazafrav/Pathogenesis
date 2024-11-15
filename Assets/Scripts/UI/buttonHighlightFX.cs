using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class ButtonHighlightFX : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ParticleSystem[] _particleSystem;

    [SerializeField] private string hoverSFXPath = "event:/UI/Menu Button Hover";
    [SerializeField] private string selectSFXPath = "event:/UI/Menu Button Select";

    private FMOD.Studio.EventInstance hoverInstance;
    private FMOD.Studio.EventInstance selectInstance;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hoverInstance = FMODUnity.RuntimeManager.CreateInstance(hoverSFXPath);
        selectInstance = FMODUnity.RuntimeManager.CreateInstance(selectSFXPath);

        if(EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().Play("ButtonHighlight");
            EventSystem.current.currentSelectedGameObject.GetComponent<ButtonHighlightFX>().PlayParticles();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);

        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
        if (animator != null)
        {
            animator.Play("ButtonHighlight");
        }

        hoverInstance.start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
        if (animator != null)
        {
            animator.Play("ButtonAnim");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
        if (animator != null)
        {
            animator.Play("ButtonHighlight");
        }

        if(GameManager.Instance.IsThereAGamepadConnected)
        {
            hoverInstance.start();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
        if (animator != null)
        {
            animator.Play("ButtonAnim");
        }
    }

    public void OnClick()
    {
        Debug.Log(gameObject.name);
        animator.Play("ButtonSelected");

        selectInstance.start();
    }

    public void PlayParticles()
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
    }

    public void PlaySelectVFX()
    {
        selectInstance.start();

    }
}
