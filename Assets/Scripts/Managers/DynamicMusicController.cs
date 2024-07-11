using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicMusicController : MonoBehaviour
{
    // 0 = MainMenu; 1 = Virus;  2 = PauseMenu;  3 = Photonic;  4 = Electric;  5 = Crystalline; 6&7 = Endings
    public AudioMixerSnapshot[] tracks;
    [SerializeField]
    private int selectionIndex = 0;
    [SerializeField]
    private float transitionSpeed = 0.5f;
 
    void Update()
    {
        tracks[selectionIndex].TransitionTo(transitionSpeed);
    }

    public void SetSelectionIndex(int index)
    {
        selectionIndex = index;
    }

    public int GetSelectionIndex()
    {
        return selectionIndex;
    }       
        
 }


