using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour, IActivatableElement
{
    public Transform pointOpen;
    public Transform pointClose;
    [SerializeField]
    public float movingSpeed = 3.0f;
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
        nextPosition = pointOpen.position;
    }

    public void Close()
    {
        nextPosition = pointClose.position;
    }
}
