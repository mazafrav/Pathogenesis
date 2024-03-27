using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitiveDoor : MonoBehaviour, IActivatableElement
{
    private Vector2 initPos = Vector2.zero;
    private Vector3 activePos = Vector2.zero;

    [SerializeField]
    public bool active = false;
    [SerializeField]
    public Vector2 activeOffset = new Vector2(0, 2);

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        activePos = initPos + activeOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        active = !active;
        if (active)
        {
            transform.position = activePos;
        }
        else
        {
            transform.position = initPos;
        }
    }

    public void Deactivate()
    {
    }
}