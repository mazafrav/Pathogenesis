using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLayerDetection : MonoBehaviour
{
    [SerializeField] private float minDistanceToAddLayer = 15.0f;
    [SerializeField] private float checkLayersCD = 2f;
    private float timer = 0;

    private CircleCollider2D detectionCollider;

    private List<RangedEnemy> rangedEnemies = new List<RangedEnemy>();
    private List<ElectricEnemy> electricEnemies = new List<ElectricEnemy>();
    private List<CrystallineEnemy> crystalineEnemies = new List<CrystallineEnemy>();

    private SoundtrackManager soundtrackManager;

    void Start()
    {
        detectionCollider = GetComponent<CircleCollider2D>();
        detectionCollider.transform.localScale = new Vector3(minDistanceToAddLayer * 2.0f, minDistanceToAddLayer * 2.0f, detectionCollider.transform.localScale.z);

    }

    private void Update()
    {
        soundtrackManager = GameManager.Instance.soundtrackManager;

        if (soundtrackManager != null)
        {
            timer += Time.deltaTime;

            if (timer >= checkLayersCD)
            {
                timer = 0;

                if (rangedEnemies.Count > 0)
                {
                    float current = 0;
                    foreach (RangedEnemy rangedEnemy in rangedEnemies)
                    {
                        float distance = Vector2.Distance(transform.parent.position, rangedEnemy.transform.position);
                        float relativeDistance = distance / minDistanceToAddLayer;
                        float approachIntensity = Mathf.Clamp(1 - relativeDistance, 0f, 1f);

                        if (approachIntensity > current)
                        {
                            current = approachIntensity;
                        }
                    }

                    //Debug.Log("RANGED INTENSITY: " + current);

                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Photogenic, current);
                }
                else
                {
                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Photogenic, 0f);
                }

                if (electricEnemies.Count > 0)
                {
                    float current = 0;
                    foreach (ElectricEnemy electricEnemy in electricEnemies)
                    {
                        float distance = Vector2.Distance(transform.parent.position, electricEnemy.transform.position);
                        float relativeDistance = distance / minDistanceToAddLayer;
                        float approachIntensity = Mathf.Clamp(1 - relativeDistance, 0f, 1f);

                        if (approachIntensity > current)
                        {
                            current = approachIntensity;
                        }
                    }

                    //Debug.Log("ELECTRIC INTENSITY: " + current);

                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Electric, current);
                }
                else
                {
                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Electric, 0f);
                }

                if (crystalineEnemies.Count > 0)
                {
                    float current = 0;
                    foreach (CrystallineEnemy crystalineEnemy in crystalineEnemies)
                    {
                        float distance = Vector2.Distance(transform.parent.position, crystalineEnemy.transform.position);
                        float relativeDistance = distance / minDistanceToAddLayer;
                        float approachIntensity = Mathf.Clamp(1 - relativeDistance, 0f, 1f);

                        if (approachIntensity > current)
                        {
                            current = approachIntensity;
                        }
                    }

                    //Debug.Log("CRYSTALLINE INTENSITY: " + current);

                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Crystalline, current);
                }
                else
                {
                    soundtrackManager.ChangeSoundtrackParameter(SoundtrackManager.SoundtrackParameter.Crystalline, 0f);
                }

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.GetComponentInParent<PlayerController>() == null)
        {
            RangedEnemy ranged = other.GetComponent<RangedEnemy>();
            if (ranged != null)
            {
                if (!rangedEnemies.Contains(ranged))
                {
                    rangedEnemies.Add(ranged);
                }
                return;
            }

            ElectricEnemy electric = other.GetComponent<ElectricEnemy>();
            if (electric != null)
            {
                if (!electricEnemies.Contains(electric))
                {
                    electricEnemies.Add(electric);
                }
                return;
            }

            CrystallineEnemy crystalline = other.GetComponent<CrystallineEnemy>();
            if (crystalline != null)
            {
                if (!crystalineEnemies.Contains(crystalline))
                {
                    crystalineEnemies.Add(crystalline);
                }
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            RangedEnemy ranged = other.GetComponent<RangedEnemy>();
            if (ranged != null)
            {
                if (rangedEnemies.Contains(ranged))
                {
                    rangedEnemies.Remove(ranged);
                }
                return;
            }

            ElectricEnemy electric = other.GetComponent<ElectricEnemy>();
            if (electric != null)
            {
                if (electricEnemies.Contains(electric))
                {
                    electricEnemies.Remove(electric);
                }
                return;
            }

            CrystallineEnemy crystalline = other.GetComponent<CrystallineEnemy>();
            if (crystalline != null)
            {
                if (crystalineEnemies.Contains(crystalline))
                {
                    crystalineEnemies.Remove(crystalline);
                }
                return;
            }

        }
        
    }

}
