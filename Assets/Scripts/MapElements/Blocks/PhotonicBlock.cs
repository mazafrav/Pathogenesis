using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicBlock : MonoBehaviour, IActivatableElement
{
    public Transform pointOpen;
    public Transform pointClose;
    [SerializeField]
    public float movingSpeed = 3.0f;
    [SerializeField]
    public Animator animator;
    public bool isOpened = false;

    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = pointClose.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, movingSpeed * Time.deltaTime);
    }
    
    public void Activate()
    {
        animator.Play("PhotonicBlockActivation");
        if (isOpened)
        {
            nextPosition = pointClose.position;
        }
        else if (!isOpened)
        {
            nextPosition = pointOpen.position;
        }
        isOpened = !isOpened;
    }

    public void Open()
    {
        animator.Play("PhotonicBlockActivation");
        nextPosition = pointOpen.position;
    }

    public void Close()
    {
        animator.Play("PhotonicBlockActivation");
        nextPosition = pointClose.position;
    }
}
