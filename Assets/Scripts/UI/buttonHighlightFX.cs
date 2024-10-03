using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ButtonHighlightFX : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ParticleSystem[] _particleSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        foreach (var particle in _particleSystem)
        {
            particle.Stop();
        }
    }

}
