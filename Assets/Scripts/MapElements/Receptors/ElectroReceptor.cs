using System.Collections;
using UnityEngine;

public class ElectroReceptor : ReceptorBase
{

    [SerializeField]
    private float timeToActivate = 2f;

    [SerializeField]
    private float lingeringTimeShock = 0.1f;

    [SerializeField]
    private float cooldownTime = 0.3f;

    [SerializeField]
    private ParticleSystem activationVFX;

    private enum Stage { IDLE, STAGE_1, STAGE_2 };
    private Stage stage = Stage.IDLE;
    private float currentTime = 0.0f;
    private float currentCooldownTime = 0.0f;
    private bool onCooldown = false;
    private bool isReceivingShock = false;
    private IEnumerator lingeringTimer;

    private new void Update()
    {
        if (!onCooldown)
        {
            if (isReceivingShock)
            {
                currentTime += Time.deltaTime;
                UpdateStage();
            }
            else
            {
                // if not receiving shock, reset state
                Reset();
            }
        }
        else
        {
            currentCooldownTime -= Time.deltaTime;
            if (currentCooldownTime < 0) { onCooldown = false; }
        }

        UpdateAnimation();
    }

    private void Reset()
    {
        stage = Stage.IDLE;
        currentTime = 0.0f;
    }

    private void UpdateStage()
    {
        if (currentTime > timeToActivate)
        // time of activation completed: activate element, reset state and start cooldown
        {
            TriggerTargets();
            activationVFX.Play();
            //VFX ACTIVATION
            Reset();
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

    //Checks current state and sets animation if necessary
    private void UpdateAnimation()
    {
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
