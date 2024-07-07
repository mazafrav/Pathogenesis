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

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip movingClip;

    private Vector3 nextPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetRespawnValues().Count == 0)
        {
            nextPosition = pointClose.position;
        }
        else
        {
            List<GameObject> gameObjectsToRespawn = GameObject.Find("RespawnLoader").GetComponent<SaveGameObjectForRespawn>().gameObjectsToSave;

            Dictionary<int, Vector3> respawnValues = GameManager.Instance.GetRespawnValues();
            for (int i = 0; i < gameObjectsToRespawn.Count; i++)
            {
                if (gameObjectsToRespawn[i] == gameObject)
                {
                    respawnValues.TryGetValue(i, out Vector3 pos);
                    if (Vector3.Distance(pos, pointClose.position) > Vector3.Distance(pos, pointOpen.position))
                    {
                        nextPosition = pointOpen.position;
                    }
                    else
                    {
                        nextPosition = pointClose.position;
                    }

                    transform.position = pos;

                    break;
                }
            }

            if (nextPosition == Vector3.zero)
            {
                nextPosition = pointOpen.position;
            }

            //nextPosition = pointOpen.position;
            //movingSpeed *= 200f;
        }


        audioSource = GetComponent<AudioSource>();
        audioSource.pitch += 0.5f;
        audioSource.clip = movingClip;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, movingSpeed * Time.deltaTime);

        if (!audioSource.isPlaying && transform.position != nextPosition)
        {
            audioSource.Play();
        }

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
