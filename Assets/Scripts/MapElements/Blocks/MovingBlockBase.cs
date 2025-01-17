using UnityEngine;

public class MovingBlockBase : MonoBehaviour, IActivatableElement
{
    [SerializeField]
    protected Transform pointOpen;
    [SerializeField]
    protected Transform pointClose;
    [SerializeField]
    protected float movingSpeed = 3.0f;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected float pitch = -0.5f;
   
    public bool IsOpened { get; set; } = false;
    public Vector3 NextPosition { get; set; } = Vector3.zero;

    private FMODUnity.StudioEventEmitter emitter;

    protected virtual void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, NextPosition, movingSpeed * Time.deltaTime);

        if (!emitter.IsPlaying() && (transform.position - NextPosition).sqrMagnitude > 0.01f) 
        {
            emitter.Play();
        }
        if (emitter.IsPlaying() && transform.position == NextPosition)
        {
            emitter.Stop();
        }
    }

    public virtual void Activate()
    {
        if (IsOpened)
        {
            NextPosition = pointClose.position;
        }
        else if (!IsOpened)
        {
            NextPosition = pointOpen.position;
        }
        IsOpened = !IsOpened;
    }

    public virtual void Open()
    {
        NextPosition = pointOpen.position;
    }

    public virtual void Close()
    {
        NextPosition = pointClose.position;
    }

    public void SetStartPosition()
    {
        NextPosition = pointClose.position;
    }
}
