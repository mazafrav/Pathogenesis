using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum STATES
{
    Capa0 = 0,
    Capa1 = 1,
    Capa2 = 2,
    Capa3 = 3,
    Capa4 = 4,
    Capa5 = 5,
}

public class DynamicMusicController : MonoBehaviour
{
  //public string[] states = new string[]{"Capa0", "Capa1", "Capa2","Capa3","Capa4","Capa5"} ;//Array of states

  public AudioMixerSnapshot[] tracks = new AudioMixerSnapshot[5];
  public int selectionIndex = 0;
  public float transitionSpeed = 0.5f;
  public STATES currentState;
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
      if (selectionIndex != (int)currentState)
      {
          selectionIndex = (int)currentState;
          tracks[selectionIndex].TransitionTo(transitionSpeed);
      }
  }
}


