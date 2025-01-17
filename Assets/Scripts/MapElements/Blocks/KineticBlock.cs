using UnityEngine;

public class KineticBlock : MovingBlockBase
{
    // Start is called before the first frame update
    //void Start()
    //{

    //    saveSystem = GameManager.Instance.GetSaveSystem();
    //    GameData gameData = saveSystem.GetGameData();
    //    sceneName = SceneManager.GetActiveScene().name;
    //    GameData.LevelData levelData = gameData.GetLevelData(sceneName);

    //    if (levelData.movingBlocks != null && levelData.movingBlocks.ContainsKey(transform.parent.name))
    //    {
    //        isOpened = levelData.movingBlocks[transform.parent.name].isOpened;
    //        nextPosition = levelData.movingBlocks[transform.parent.name].pos.ConvertToUnityType();
    //        transform.position = nextPosition;
    //    }
    //    else
    //    {
    //        nextPosition = pointClose.position;
    //    }

    //    saveSystem.onSave += OnSave;


    //    emitter = GetComponent<FMODUnity.StudioEventEmitter>();

    //}

    public override void Activate()
    {
        base.Activate();
        animator.Play("KineticBlockActivation");
    }

    public override void Open()
    {
        base.Open();
        animator.Play("KineticBlockActivation");
    }

    public override void Close()
    {
        base.Close();
        animator.Play("KineticBlockActivation");
    }

    //private void OnSave()
    //{
    //    GameData.LevelData levelData = saveSystem.GetGameData().GetLevelData(sceneName);

    //    GameData.MovingBlockData blockInfo = new GameData.MovingBlockData();
    //    blockInfo.pos.ConvertToPosType(nextPosition.x, nextPosition.y);
    //    blockInfo.isOpened = isOpened;

    //    levelData.AddMovingBlockData(transform.parent.name, blockInfo);

    //    saveSystem.SaveCurrentLevelState(sceneName, levelData);
    //}

    //void OnDisable()
    //{
    //    saveSystem.onSave -= OnSave;
    //}
}
