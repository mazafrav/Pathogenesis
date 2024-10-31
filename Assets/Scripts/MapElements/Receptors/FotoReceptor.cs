using UnityEngine;

public class FotoReceptor : ReceptorBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FotoBullet"))
        {
            EmitSFX();
            TriggerTargets();
            GetComponentInParent<Animator>().Play("FotoReceptorDeactAnim");
        }
    }
}
