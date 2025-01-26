using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Type,Type> OnAttackSameSpecie;

    [SerializeField]
    protected HostLocomotion locomotion;
    [Header("Movement")]
    [SerializeField]
    protected Transform[] wayPoints;
    [SerializeField]
    protected float chaseSpeed = 3.0f;
    [SerializeField]
    protected float patrolSpeed = 3.0f;

    [SerializeField]
    private float minDistanceToWaypoint = 0.3f;
    [Header("VFX")]
    [SerializeField]
    private ParticleSystem deathEffect;

    protected float movementDirection = 0;

    protected int currentWayPointIndex = 0;

    [Header("SFX")]
    [SerializeField]
    private string detectEventPath;
    [SerializeField]
    private float pitch;
    [SerializeField]
    private bool shouldApplyDangerLayer = false;
    [SerializeField]
    [Range(0f, 1f)]
    private float dangerLayerIntensity;
    [SerializeField]
    private GameObject layerDetectionCollider;

    private FMOD.Studio.EventInstance detectEventInstance;
    private List<GameObject> organismsDetected = new List<GameObject>();
    private Enemy possessedEnemy = null;


    public bool IsPossesed { get; set; } = false;

    private void Awake()
    {
        detectEventInstance = FMODUnity.RuntimeManager.CreateInstance(detectEventPath);
    }
    public bool CanAttackSameSpecie { get; set; } = false;

    virtual protected void Start()
    {
        OnAttackSameSpecie += AllowAttackSameSpecies;

        layerDetectionCollider.SetActive(false);
    }

    public void SetLayerDetection (bool value)
    {
        layerDetectionCollider.SetActive(value);
    }

    virtual public void DestroyEnemy()
    {
        possessedEnemy = GameManager.Instance.GetPlayerController().GetComponentInParent<Enemy>();

        //If the player is possessing an enemy we notify the others electric enemies
        if (possessedEnemy)
        {
            possessedEnemy.transform.position += new Vector3(0.01f, 0.0f, 0.0f); //We need to move it a bit so OnTriggerStay is executed 

            OnAttackSameSpecie?.Invoke(GetType(), possessedEnemy.GetType());
        }

        GameManager.Instance.GetPlayerController().OnLeaveAbsorbableRange();
               
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

   virtual protected void Patrol()
   {
        if (wayPoints[currentWayPointIndex])
        {
            locomotion.SetMoveSpeed(patrolSpeed);

            Vector2 dirToWaypoint = (wayPoints[currentWayPointIndex].position - transform.position).normalized;
            movementDirection = dirToWaypoint.x;
            float distanceToWayPoint = (transform.position - wayPoints[currentWayPointIndex].position).magnitude;
            locomotion.Move(dirToWaypoint.x, dirToWaypoint.y);

            if (distanceToWayPoint < minDistanceToWaypoint)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
            }
        }
   }

    public Transform[] GetWayPoints() { return wayPoints; }
    public void SetWayPoints(Transform[] _waypoints) { wayPoints = _waypoints; }

    public void CheckIfDetected (GameObject organism)
    {
        organismsDetected.RemoveAll(obj => obj == null); //Remove null elements
        if (organismsDetected.Contains(organism))
        {
            return;
        }

        organismsDetected.Add(organism);
        detectEventInstance.start();
        detectEventInstance.getPitch(out float originalPitch);
        detectEventInstance.setPitch(originalPitch + pitch);

        if (shouldApplyDangerLayer && organism.GetComponentInChildren<PlayerController>())
        {
            if (organism.CompareTag("Enemy") && !CanAttackSameSpecie)
            {
                return;
            }
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, Mathf.Clamp(dangerLayerIntensity, 0f, 1f));
        }
    }

    protected void AllowAttackSameSpecies(Type deadEnemyType, Type possessedEnemyType)
    {       
        if (deadEnemyType == possessedEnemyType)
        {
            CanAttackSameSpecie = true;
        }
    }

    private void OnDisable()
    {
        OnAttackSameSpecie -= AllowAttackSameSpecies;
    }
}
