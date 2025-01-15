using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageControl : MonoBehaviour
{
    [SerializeField] float invencibilityTime = 0.5f;
    private float currentInvecibilityTime = 0.5f;
    private bool wasPossessing = false;

    // Start is called before the first frame update
    private void Start()
    {
        currentInvecibilityTime = invencibilityTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (wasPossessing)
        {
            currentInvecibilityTime -= Time.deltaTime;
        }

        if (currentInvecibilityTime <= 0.0f)
        {
            currentInvecibilityTime = invencibilityTime;
            wasPossessing = false;
        }
    }

    public void Damage(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !wasPossessing)
        {
            PlayerController playerController = collider.GetComponentInChildren<PlayerController>();
            playerController.PlayerBodyDeath();
            GameManager.Instance.GetLevelLoader().CheckRespawn();
        }
        else if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            PlayerController playerController = collider.GetComponentInChildren<PlayerController>();
            if (enemy)
            {
                if (enemy.IsPossesed && playerController) //The enemy is possessed
                {
                    wasPossessing = true;

                    //Change player controller parent to the player
                    GameObject virus = GameManager.Instance.GetPlayer();
                    playerController.transform.parent = virus.transform;
                    virus.SetActive(true);

                    //Setting player position to the enemy position
                    Vector3 enemyPos = enemy.transform.position;
                    enemy.DestroyEnemy();
                    virus.transform.position = enemyPos;
                    
                    //Puede que no haga falta
                    GameManager.Instance.GetCamera().Follow = playerController.GetPlayerBody().transform;
                    GameManager.Instance.GetPlayerController().locomotion = GameManager.Instance.GetPlayerLocomotion();
                    //GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Absorption, 0);
                    GameManager.Instance.soundtrackManager.ResetSoundtrack();
                    GameManager.Instance.GetPlayerController().onHostDeath?.Invoke();
                }
                else //The enemy is not possessed
                {
                    enemy.DestroyEnemy();
                    GameManager.Instance.soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Danger, 0f);
                }

            }
        }
    }





}
