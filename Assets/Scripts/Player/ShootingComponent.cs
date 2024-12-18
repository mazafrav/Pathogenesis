using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingComponent : MonoBehaviour
{
    [SerializeField]
    private float deadZone = 0.5f;

    private bool flipRot = true;
    public bool bisActive { get; set; } = true;

    private Enemy enemyIA;

    private void Start()
    {
        enemyIA = GetComponentInParent<Enemy>();
    }

    public void Aim(Vector2 target)
    {
        // if(!bisActive) return;

        if (enemyIA.enabled || (!enemyIA.enabled && !GameManager.Instance.IsThereAGamepadConnected && !GameManager.Instance.GetPlayerController().isPossessing)) //The enemy AI is active or is disabled with no gamepads connected
        {
            transform.up = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        }
        else if (GameManager.Instance.IsThereAGamepadConnected && !GameManager.Instance.GetPlayerController().isPossessing)
        {
            if ((target.x > deadZone || target.x < -deadZone) || (target.y > deadZone || target.y < -deadZone))
            {
                float angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
                angle = flipRot ? -angle : angle;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }  
}
