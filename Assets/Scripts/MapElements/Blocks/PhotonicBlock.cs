using UnityEngine;

public class PhotonicBlock : MovingBlockBase
{
    public override void Activate()
    {
        base.Activate();
        animator.Play("PhotonicBlockActivation");       
    }

    public override void Open()
    {
        base.Open();
        animator.Play("PhotonicBlockActivation");
    }

    public override void Close()
    {
        base.Close();
        animator.Play("PhotonicBlockActivation");
    }

}
