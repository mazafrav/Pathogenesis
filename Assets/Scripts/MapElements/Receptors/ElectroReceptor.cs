using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroReceptor : MonoBehaviour
{
    [SerializeField]
    public GameObject activatableElement;

    [SerializeField]
    private float timeToActivate = 2f;

    [SerializeField]
    private float lingeringTimeShock = 0.1f;

    [SerializeField]
    private float cooldownTime = 0.3f;


    [SerializeField]
    private AudioClip activateClip;

    enum Stage { IDLE, STAGE_1, STAGE_2 };
    private Stage stage = Stage.IDLE;
    private float currentTime = 0.0f;
    private float currentCooldownTime = 0.0f;
    public bool onCooldown { get; private set; } = false;
    public bool isReceivingShock { get; private set; } = false;
    private IEnumerator lingeringTimer;
    private AudioSource audioSource;

    private IActivatableElement activatableInterface;
    // Start is called before the first frame update
    private void Start()
    {
        //currentTimeToBeActivatedAgain = timeToBeActivatedAgain;
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch -= 0.5f;
    }

    private void Update()
    {
        if (!onCooldown)
        {
            if (isReceivingShock)
            {
                currentTime += Time.deltaTime;
                if (currentTime > timeToActivate)
                // time of activation completed: activate element, reset state and start cooldown
                {
                    activatableInterface.Activate();
                    //VFX ACTIVATION
                    stage = Stage.IDLE;
                    currentTime = 0.0f;
                    isReceivingShock = false;
                    StopCoroutine(lingeringTimer);
                    onCooldown = true;
                    currentCooldownTime = cooldownTime;

                }
                else if (currentTime > timeToActivate / 2 && currentTime <= timeToActivate)
                {
                    stage = Stage.STAGE_2;
                }
                else if (currentTime > 0 && currentTime <= timeToActivate / 2)
                {
                    stage = Stage.STAGE_1;
                }
            }
            else
            {
                // if not receiving shock, reset state
                stage = Stage.IDLE;
                currentTime = 0.0f;
            }
        }
        else
        {
            currentCooldownTime -= Time.deltaTime;
            if (currentCooldownTime < 0) { onCooldown = false; }
        }

        //Checks current state and sets animation if necessary
        AnimatorStateInfo animatorStateInfo = GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (stage == Stage.IDLE && !animatorStateInfo.IsName("ElectroReceptor-Idle"))
        {
            GetComponentInParent<Animator>().Play("ElectroReceptor-Idle");
        }
        else if (stage == Stage.STAGE_1 && !animatorStateInfo.IsName("ElectroReceptor-Stage1"))
        {
            GetComponentInParent<Animator>().Play("ElectroReceptor-Stage1");
        }
        else if (stage == Stage.STAGE_2 && !animatorStateInfo.IsName("ElectroReceptor-Stage2"))
        {
            GetComponentInParent<Animator>().Play("ElectroReceptor-Stage2");
        }
    }

    public void ElectroShock()
    {
        // if lingering timer is active, kill it
        if (lingeringTimer != null) { StopCoroutine(lingeringTimer); }
        
        isReceivingShock = true;
       
        // reset lingering timer
        lingeringTimer = LingeringShockTimer();
        StartCoroutine(lingeringTimer);
    }

    public IEnumerator LingeringShockTimer()
    {
        //after lingerTimeShock seconds, sets isReceving shock to false
        yield return new WaitForSeconds(lingeringTimeShock);
        isReceivingShock = false;
    }
}
