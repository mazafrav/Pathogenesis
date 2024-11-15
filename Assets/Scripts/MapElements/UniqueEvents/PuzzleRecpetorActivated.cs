using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRecpetorActivated : MonoBehaviour, IActivatableElement
{
    [SerializeField] int soundToPlay = 0;

    private bool doOnce = true;

    public void Activate()
    {
        if (doOnce)
        {
            GetComponent<SolvedPuzzleManager>().PlaySound(soundToPlay);
            doOnce = false;
        }
    }

    public void Open()
    {

    }

    public void Close()
    {

    }
}
