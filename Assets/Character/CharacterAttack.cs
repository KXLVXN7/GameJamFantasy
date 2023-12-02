using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public float attackPower;
    public float cooldown;
    public float criticalChance;
    public float attackRange;
    public LayerMask enemyLayer;

    private float lastAttackTime;

    private void Start()
    {
        lastAttackTime = -cooldown; // Set initial value to allow immediate attack
    }

    private void Update()
    {
        // For testing purposes, you can call PerformAttack() when a certain input (e.g., mouse click) is detected.
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        if (Time.time - lastAttackTime > cooldown)
        {
            lastAttackTime = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            if (hitColliders.Length > 0)
            {
                // Choose a random enemy from the detected enemies
                Collider randomEnemyCollider = hitColliders[Random.Range(0, hitColliders.Length)];

                // Check if the hit object has the EnemyHealth component
                EnemyHealth enemyHealthComponent = randomEnemyCollider.GetComponent<EnemyHealth>();
                if (enemyHealthComponent != null)
                {
                    float damage = CalculateDamage();
                    bool isCritical = IsCriticalHit();

                    // Call the TakeDamageEnemy function on the EnemyHealth component
                    enemyHealthComponent.TakeDamageEnemy(damage, isCritical);
                }
            }
        }
    }

    private float CalculateDamage()
    {
        // You can implement a more sophisticated damage calculation based on attackPower, etc.
        return attackPower;
    }

    private bool IsCriticalHit()
    {
        // Check if the attack results in a critical hit based on criticalChance
        float randomValue = Random.value;
        return randomValue < criticalChance;
    }
}
