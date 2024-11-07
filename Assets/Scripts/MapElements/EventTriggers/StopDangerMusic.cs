using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDangerMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>()) )
        {
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, 0f);
        }
        
    }
}
