using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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


    private void Awake()
    {
        detectEventInstance = FMODUnity.RuntimeManager.CreateInstance(detectEventPath);
    }
    public bool CanAttackSameSpecie { get; set; } = false;

    virtual protected void Start()
    {
        layerDetectionCollider.SetActive(false);
    }

    public void SetLayerDetection (bool value)
    {
        layerDetectionCollider.SetActive(value);
    }

    virtual public void DestroyEnemy()
    {
        GameManager.Instance.GetPlayerController().OnLeaveAbsorbableRange();
        if (locomotion.GetType() == typeof(ElectricLocomotion))
        {
            ElectricLocomotion electric = (ElectricLocomotion)locomotion;

            electric.StopJumpLoopSFX();
        }
        
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

   virtual protected void Patrol()
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

        if (shouldApplyDangerLayer && organism.GetComponentInParent<PlayerController>())
        {
            if (organism.CompareTag("Enemy") && !CanAttackSameSpecie)
            {
                return;
            }
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, Mathf.Clamp(dangerLayerIntensity, 0f, 1f));
        }
    }

    protected void AllowAttackSameSpecies()
    {
        CanAttackSameSpecie = true;
    }

}
