using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectroBlock : MovingBlockBase
{ 
    public override void Activate()
    {
        base.Activate();
        animator.Play("ElectroBlockActivation");       
    }

    public override void Open()
    {
        base.Open();    
        animator.Play("ElectroBlockActivation");
    }

    public override void Close()
    {
        base.Close();
        animator.Play("ElectroBlockActivation");
    }   
}
