using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameObjectForRespawn : MonoBehaviour
{
    public List<GameObject> gameObjectsToSave;
    // Start is called before the first frame update
    void Awake()
    {
        //GameManager.Instance.AddGameObjectToRespawn(gameObjectsToSave);
    }
}
