using UnityEngine;

public class MovingBlock : MovingBlockBase
{
    public override void Activate()
    {
        base.Activate();
        animator.Play("MovingBlockActivation");
    }

    public override void Open()
    {
        base.Open();
        animator.Play("MovingBlockActivation");
    }

    public override void Close()
    {
        base.Close();
        animator.Play("MovingBlockActivation");
    }
}
