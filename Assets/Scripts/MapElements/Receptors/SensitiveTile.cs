using UnityEngine;

public class SensitiveTile : ReceptorBase
{
    //ONE WAY TRIGGER
    //Will only trigger if exiting in the same direction that the body entered the collision

    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    private Vector2 velocityEnter = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        velocityEnter = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Vector2 velocityExit = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
        if (Vector2.Dot(velocityEnter, velocityExit) > 0)
        {
            animator.Play("SensitiveTileActivated");
            TriggerTargets();
        }
    }
}
