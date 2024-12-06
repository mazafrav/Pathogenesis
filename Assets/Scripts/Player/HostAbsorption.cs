using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class HostAbsorption : Interactable
{
    [HideInInspector] public bool interactable = true;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private SpriteRenderer graphics;
    [SerializeField]
    private SpriteRenderer weaponGraphics;

    [Header("Possession Effect")]
    [SerializeField]
    private float possessionEffectTime = 1.5f;
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

    private float possessionTimer = 0.0f;

    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;
    private Enemy enemyBehaviour;

    private bool doOnce = false;
    public Color possessingColor => possessColor;

    void Start()
    {
        SetInfo();
        Physics2D.queriesStartInColliders = false;
        possessionEventInstance = FMODUnity.RuntimeManager.CreateInstance(possessionEventPath);
    }

    private void SetInfo()
    {
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        playerLocomotion = GameManager.Instance.GetPlayerLocomotion();
        enemyBehaviour = GetComponent<Enemy>();
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
        }
        else
        {
            if (doOnce)
            {
                playerController.isPossessing = false;
                //CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
                //cinemachineVirtualCamera.GetComponent<PossessionPostProcess>().isActive = false;
                //Debug.Log("absooorption");
                GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Absorption, 0.5f);
                doOnce = false;
            } 
           
        }
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        if (!interactable) 
            return;

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
        //else if (/*!playerLOS &&*/ playerController.AbsorbableHostInRange == this)
        //{
        //    //Debug.LogWarning("disable absorbable");
        //    playerController.OnLeaveAbsorbableRange();
        //}
    }

    protected override void OnDeactivateAbsorptionVFX()
    {
        playerController.OnLeaveAbsorbableRange();
        playerController.StopAbsorbableSFX();
    }

    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);

        if (playerController.GetPlayerBody()) //we possess if the player exists
        {
            ApplyPossessionWithNoEffects();
            return;

            playerLocomotion.DisableFreeMovement();
            hostLocomotion.ResetAttack();
            hostLocomotion.StopSFX();
            hostLocomotion.SetPossessingParameters();
            playerController.locomotion = hostLocomotion;
            //TODO: cambiar  (?)
            gameObject.transform.parent = playerController.transform;
            //playerController.locomotion.Set3DAttributes(gameObject);
            playerController.DisablePlayerBody();
            playerController.OnPossesionEvent(hostLocomotion);
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
            ApplyEnemySoundtrackLayer();
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Absorption, 1);
            GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, 0);
            hostLocomotion.GetComponent<Enemy>().SetLayerDetection(true);

            doOnce = true;
            //cinemachineVirtualCamera.GetComponent<PossessionPostProcess>().isActive = true;
            //
            enemyBehaviour.enabled = false;
        }

    }

    public void ApplyPossessionWithNoEffects()
    {
        SetInfo();
        if (playerController.GetPlayerBody()) //we possess if the player exists
        {
            playerLocomotion.DisableFreeMovement();
            hostLocomotion.ResetAttack();
            hostLocomotion.SetPossessingParameters();
            playerController.locomotion = hostLocomotion;
            Debug.Log(playerController.locomotion);
            playerController.transform.parent.gameObject.SetActive(false);
            playerController.transform.parent = gameObject.transform;
            playerController.DisablePlayerBody();
            GameManager.Instance.SetPlayer(gameObject);


            //TODO: maybe no hace falta si se sigue al playercontroller todo el rato
            CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
            if (cinemachineVirtualCamera != null)
            {
                CameraSwitchManagement cameraSwitchManagement = cinemachineVirtualCamera.GetComponent<CameraSwitchManagement>();

                cameraSwitchManagement?.setNewFollow(hostLocomotion.transform);
                //cameraSwitchManagement?.StartPossessionEffect(possessionEffectTime);
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


            //possessionTimer = possessionEffectTime;

            //hostLocomotion.GetOneShotSource().PlayOneShot(possessionClip);

            //possessionEventInstance.start();
            ApplyEnemySoundtrackLayer();
            //GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Absorption, 1);
            //GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, 0);
            hostLocomotion.GetComponent<Enemy>().SetLayerDetection(true);

            /*
            currentTimeToZoomIn = possessionEffectTime/2.0f;
            currentTimeToZoomOut = possessionEffectTime/2.0f;
            */
            //doOnce = true;
            //cinemachineVirtualCamera.GetComponent<PossessionPostProcess>().isActive = true;

            enemyBehaviour.IsPossesed = true;
            enemyBehaviour.enabled = false;
            
        }
    }

    private void ApplyEnemySoundtrackLayer()
    {
        SoundtrackManager soundtrackManager = GameManager.Instance.soundtrackManager;
        if (hostLocomotion.GetType() == typeof(RangedLocomotion))
        {
            soundtrackManager.PhotonicLayerIntensity = 1f;
            soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Photegenic, 1f);
        }
        else if (hostLocomotion.GetType() == typeof(ElectricLocomotion))
        {
            soundtrackManager.ElectricLayerIntensity = 1f;
            soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Electric, 1f);
        }
        else if (hostLocomotion.GetType() == typeof(CrystallineLocomotion))
        {
            soundtrackManager.CrystallineLayerIntensity = 1f;
            soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Crystalline, 1f);
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
