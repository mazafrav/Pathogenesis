using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LevelLoader levelLoader;

    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
    }

    public PlayerLocomotion GetPlayerLocomotion()
    {
        return player.GetComponent<PlayerLocomotion>();
    }

    public LevelLoader GetLevelLoader()
    {
        return levelLoader;
    }
}
