using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrsytallineBouncinessChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject bounciness;
    [SerializeField]
    private CrystalineEnemy crystallineenemy;

    private void Start()
    {
        bounciness.SetActive(false);
    }

    private void Update()
    {
        if(gameObject.activeInHierarchy && (GameManager.Instance.GetPlayerController().GetDeltaY() < 0.0f || GameManager.Instance.GetPlayerController().GetDeltaX() != 0.0f))
        {
            bounciness.SetActive(false);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("TileMap") && !crystallineenemy.enabled)
       {
            bounciness.SetActive(true);
       }
    }
}
