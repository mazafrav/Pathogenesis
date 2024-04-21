using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    [SerializeField]
    public Animator camAnim;
    public static bool isCutscenePlaying = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camAnim.SetBool("Cutscene1", true);
            Invoke(nameof(StopCutscene), 3f);
        }
    }

    void StopCutscene()
    {
        camAnim.SetBool("Cutscene1", false);
    }
}
