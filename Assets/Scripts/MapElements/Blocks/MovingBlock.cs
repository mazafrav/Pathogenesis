using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingBlock : MonoBehaviour, IActivatableElement
{
    public Transform pointOpen;
    public Transform pointClose;
    [SerializeField]
    public float movingSpeed = 3.0f;
    [SerializeField]
    public Animator animator;
    public bool isOpened = false;

    private FMODUnity.StudioEventEmitter emitter;

    private Vector3 nextPosition = Vector3.zero;

    private SaveSystem saveSystem;
    private string sceneName;
    

    // Start is called before the first frame update
    void Start()
    {
        saveSystem = GameManager.Instance.GetSaveSystem();
        GameData gameData = saveSystem.GetGameData();
        sceneName = SceneManager.GetActiveScene().name;
        GameData.LevelData levelData = gameData.GetLevelData(sceneName);

        if(levelData.movingBlocks != null && levelData.movingBlocks.ContainsKey(transform.parent.name))
        {
            isOpened = levelData.movingBlocks[transform.parent.name].isOpened;
            nextPosition = levelData.movingBlocks[transform.parent.name].pos.ConvertToUnityType();
            transform.position = nextPosition;
        }
        else 
        {
            nextPosition = pointClose.position;
        }

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        saveSystem.onSave += OnSave;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, movingSpeed * Time.deltaTime);

        if (!emitter.IsPlaying() && (transform.position - nextPosition).sqrMagnitude > 0.01f) //transform.position != nextPosition
        {
            emitter.Play();
        }
        if (emitter.IsPlaying() && transform.position == nextPosition)
        {
            emitter.Stop();
        }
    }
    
    public void Activate()
    {
        animator.Play("MovingBlockActivation");
        if (isOpened)
        {
            nextPosition = pointClose.position;
        }
        else if (!isOpened)
        {
            nextPosition = pointOpen.position;
        }
        isOpened = !isOpened;
    }

    public void Open()
    {
        animator.Play("MovingBlockActivation");
        nextPosition = pointOpen.position;
    }

    public void Close()
    {
        animator.Play("MovingBlockActivation");
        nextPosition = pointClose.position;
    }

    private void OnSave()
    {
        GameData.LevelData levelData = saveSystem.GetGameData().GetLevelData(sceneName);

        GameData.MovingBlockData blockInfo = new GameData.MovingBlockData();
        blockInfo.pos.ConvertToPosType(nextPosition.x, nextPosition.y);
        blockInfo.isOpened = isOpened;

        levelData.AddMovingBlockData(transform.parent.name, blockInfo);

        saveSystem.SaveCurrentLevelState(sceneName, levelData);
    }

    void OnDisable()
    {
        saveSystem.onSave -= OnSave;
    }
}
