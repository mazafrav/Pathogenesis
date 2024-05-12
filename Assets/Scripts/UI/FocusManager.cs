using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FocusManager : MonoBehaviour
{
    [SerializeField]
    private EventSystem eventSystem;

    private GameObject lastSelected;

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            if (lastSelected != null)
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
        else
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
    }
}
