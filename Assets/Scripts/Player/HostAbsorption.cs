using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HostAbsorption : Interactable
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private SpriteRenderer graphics;
    [SerializeField]
    private SpriteRenderer weaponGraphics;

    [Header("Possession Effect")]
    [SerializeField]
    private float cameraShakeIntensity = 0.6f;
    [SerializeField]
    private float possessionEffectTime = 1.5f;
    [SerializeField]
    private float zoomValue = 0.0f;
    [SerializeField]
    private float controllerVibrationIntestity = 0.01f;
    [SerializeField]
    private ParticleSystem absortionParticles;
    [SerializeField]
    private Color possessColor = new Color(0.6696f, 0.7624f, 0.7981f);
    [SerializeField]
    private ParticleSystem possessParticles;

    [Header("SFX")]
    [SerializeField] 
    private string possessionEventPath = "event:/SFX/Player/Player Enemy Possession";
    private FMOD.Studio.EventInstance possessionEventInstance;

    private float originalZoom;
    private float possessionTimer = 0.0f;
    private float currentTimeToZoomIn = 0.0f;
    private float currentTimeToZoomOut = 0.0f;

    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;
    private Enemy enemyBehaviour;

    private bool doOnce = false;

    public Color possessingColor => possessColor;

    void Start()
    {
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        playerLocomotion = GameManager.Instance.GetPlayerLocomotion();
        enemyBehaviour = GetComponent<Enemy>();
        Physics2D.queriesStartInColliders = false;
        possessionEventInstance = FMODUnity.RuntimeManager.CreateInstance(possessionEventPath);
    }

    protected override void Update()
    {
        base.Update();
        if (possessionTimer > 0f)
        {
            playerController.isPossessing = true;
            possessionTimer = Mathf.Max(possessionTimer - Time.deltaTime, 0f);

            ChangeColor(Color.Lerp(graphics.color, possessingColor, 1 - possessionTimer / possessionEffectTime));
            ChangePossessionMaterial(Mathf.Clamp(1 - possessionTimer / possessionEffectTime, 0f, 1f));

            /*
            if (zoomValue > 0f)
            {
                if (currentTimeToZoomIn > 0.0f) //Zoom in
                {
                    currentTimeToZoomIn = Math.Max(currentTimeToZoomIn - Time.deltaTime, 0.0f);
                  
                    Zoom(Mathf.Lerp(originalZoom, zoomValue, Mathf.Clamp(1 - currentTimeToZoomIn / (possessionEffectTime/2.0f), 0f, 1f)));
                }
                else if (currentTimeToZoomIn <= 0.0f && currentTimeToZoomOut > 0.0f) //Zoom out
                {
                    currentTimeToZoomOut = Math.Max(currentTimeToZoomOut - Time.deltaTime, 0.0f);
                                     
                    Zoom(Mathf.Lerp(zoomValue, originalZoom, Mathf.Clamp(1 - currentTimeToZoomOut / (possessionEffectTime / 2.0f), 0f, 1f)));
                }
            }
            */

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(controllerVibrationIntestity, controllerVibrationIntestity);
            }

        }
        else
        {
            if (doOnce)
            {
                playerController.isPossessing = false;
        
                Gamepad gamepad = Gamepad.current;
                if (gamepad != null)
                {
                    gamepad.SetMotorSpeeds(0f, 0f);
                }
                //CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
                //cinemachineVirtualCamera.GetComponent<PossessionPostProcess>().isActive = false;
                doOnce = false;
            } 
           
        }
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {                          
                OnInteract(collidedObject);
            }
        }      
    }

    protected override void OnActivateAbsorptionVFX(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);
        /*
        bool playerLOS = false;
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {                              
                playerLOS = true;               
            }
        }
        */
        if (/*playerLOS &&*/ playerController.AbsorbableHostInRange == null)
        {
            // Debug.LogWarning(playerLOS + " : " + playerController.AbsorbableHostInRange);
            playerController.OnEnterAbsorbableRange(this);
        }
        else if (/*!playerLOS &&*/ playerController.AbsorbableHostInRange == this)
        {
            //Debug.LogWarning("disable absorbable");
            playerController.OnLeaveAbsorbableRange();
        }
    }

    protected override void OnDeactivateAbsorptionVFX()
    {
        playerController.OnLeaveAbsorbableRange();
    }

    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);

        if (playerController.GetPlayerBody()) //we possess if the player exists
        {
            playerLocomotion.DisableFreeMovement();
            hostLocomotion.ResetAttack();
            hostLocomotion.SetPossessingParameters();
            playerController.locomotion = hostLocomotion;
            gameObject.transform.parent = playerController.transform;
            playerController.DisablePlayerBody();
            //graphics.color = possessingColor;

            ParticleSystem absortionVFX = Instantiate(absortionParticles, this.gameObject.transform.position, Quaternion.identity);
            ParticleSystem particlesVFX = Instantiate(possessParticles, this.gameObject.transform.position, Quaternion.identity);
            CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
            if (cinemachineVirtualCamera != null)
            {
                CameraSwitchManagement cameraSwitchManagement = cinemachineVirtualCamera.GetComponent<CameraSwitchManagement>();

                cameraSwitchManagement?.setNewFollow(hostLocomotion.transform);
                cameraSwitchManagement?.StartPossessionEffect(possessionEffectTime);
            }

            RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                //weaponGraphics.color = possessingColor;
                playerController.shootingComponent = rangedEnemy.shootingComponent;
                rangedEnemy.GetComponent<LineRenderer>().enabled = false;
                rangedEnemy.ResetRigidbodyConstraints();
                rangedEnemy.SetAimBehaviour(true);
            }

            ElectricEnemy electricEnemy = GetComponent<ElectricEnemy>();
            if (electricEnemy != null)
            {
                playerController.shootingComponent = electricEnemy.GetShootingComponent();
            }


            possessionTimer = possessionEffectTime;

            //hostLocomotion.GetOneShotSource().PlayOneShot(possessionClip);

            possessionEventInstance.start();

            /*
            currentTimeToZoomIn = possessionEffectTime/2.0f;
            currentTimeToZoomOut = possessionEffectTime/2.0f;
            */
            doOnce = true;
            //cinemachineVirtualCamera.GetComponent<PossessionPostProcess>().isActive = true;

            /*
            if (zoomValue > 0.0f) // Transformation of a natural zoom value chosen by arrobaManu to a practical zoom value
            {
                if (cinemachineVirtualCamera)
                {
                    originalZoom = cinemachineVirtualCamera.m_Lens.OrthographicSize;
                }
                zoomValue = Mathf.Clamp(originalZoom - zoomValue, 1 , 20);
            }
            */
            

            enemyBehaviour.enabled = false;
        }

    }

    private void ChangeColor(Color color)
    {
        graphics.color = color;
        if (weaponGraphics)
        {
            weaponGraphics.color = color;
        }
        //RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
        //if (rangedEnemy != null)
        //{
        //    weaponGraphics.color = color;
        //}
    }

    private void ChangePossessionMaterial(float value)
    {
        graphics.material.SetFloat("_Progress", value);
        if(weaponGraphics && GetComponent<RangedEnemy>() == null)
        {
            weaponGraphics.material.SetFloat("_Progress", value);
        }
        //RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
        //if (rangedEnemy != null)
        //{
        //    weaponGraphics.material.SetFloat("_Progress", value);
        //}
    }

    private void Zoom(float value)
    {
        CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();

        cinemachineVirtualCamera.m_Lens.OrthographicSize = value;
    }
}
