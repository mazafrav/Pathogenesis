using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicMusicController : MonoBehaviour
{
    public AudioMixerSnapshot[] tracks = new AudioMixerSnapshot[5];
    [SerializeField]
    private int selectionIndex = 0;
    [SerializeField]
    private float transitionSpeed = 0.5f;
 
    public static DynamicMusicController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
          
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

