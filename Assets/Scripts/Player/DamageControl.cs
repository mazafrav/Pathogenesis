using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageControl : MonoBehaviour
{

    [SerializeField] float invencibilityTime = 0.5f;
    private float currentInvecibilityTime = 0.5f;
    private bool wasPossessing = false;

    private LevelLoader levelLoader;
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
            GameManager.Instance.GetPlayerController().PlayerBodyDeath();
            GameManager.Instance.GetLevelLoader().RestartLevel();
        }
        else if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy)
            {
                if (enemy.transform.parent != null) //The enemy is possessed
                {
                    wasPossessing = true;
                    Debug.Log("Possessed");
                    enemy.transform.parent = null;
                    Vector3 enemyPos = enemy.transform.position;
                    enemy.DestroyEnemy();
                    GameManager.Instance.GetPlayerController().transform.position = enemyPos;
                    GameManager.Instance.GetPlayerController().GetPlayerBody().transform.localPosition = Vector3.zero;

                    GameManager.Instance.GetPlayerController().EnablePlayerBody();
                    GameManager.Instance.GetCamera().Follow = GameManager.Instance.GetPlayerController().GetPlayerBody().transform;
                    GameManager.Instance.GetPlayerController().locomotion = GameManager.Instance.GetPlayerLocomotion();
                }
                else //The enemy is not possessed
                {
                    enemy.DestroyEnemy();
                }

            }
        }
    }
}
