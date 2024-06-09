using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectricShock : MonoBehaviour
{
    [SerializeField]
    ElectricShockRange range;
    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;
    [SerializeField]
    private Sprite[] electricAttackSprites;
    [SerializeField]
    private float[] scaleByStep;
    private int numberOfSteps;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);
    protected bool interacted = false;
    private DamageControl damageControl;
    private SpriteRenderer spriteRenderer;
    private int currentStep = 0;
    private float stepTime, currentStepTime = 0f;
    private float attackRange = 1.0f;

    private void Awake()
    {
        damageControl = GetComponentInParent<DamageControl>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        numberOfSteps = electricAttackSprites.Length;
        attackRange = GetComponentInParent<ElectricLocomotion>().ElectricShockRange;
        GetComponentInParent<CircleCollider2D>().radius = 1f; ;
        stepTime = GetComponentInParent<ElectricLocomotion>().GetShockDuration() / (numberOfSteps);
    }

    private void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            if (o.gameObject == range.personInRange)
            {             
               damageControl.Damage(o);
            }   
            else if (o.gameObject.GetComponent<ElectroReceptor>() != null)
            {
                o.gameObject.GetComponent<ElectroReceptor>().ElectroShock();
            }
            else if(o.gameObject.GetComponent<CrystalBlock>() != null)
            {
                o.gameObject.GetComponent<CrystalBlock>().DestroyCrystalBlock();
            }
        }
        currentStepTime += Time.deltaTime;
        if (currentStepTime > stepTime)
        {
            currentStepTime = 0f;
            SetNextStep();

        }
    }

    void OnEnable()
    {
        currentStep = 1;
        currentStepTime = 0f;
        spriteRenderer.sprite = electricAttackSprites[currentStep];
    }

    private void SetNextStep()
    {
        currentStep = currentStep + 1;

        
        if (currentStep <= numberOfSteps)
        {
            spriteRenderer.sprite = electricAttackSprites[currentStep - 1];
            GetComponentInParent<CircleCollider2D>().transform.localScale = new Vector3(attackRange * scaleByStep[currentStep - 1], attackRange * scaleByStep[currentStep - 1], 0);
        }
    }

    public int GetNumberOfSprites()
    {
        return numberOfSteps;
    }
}
