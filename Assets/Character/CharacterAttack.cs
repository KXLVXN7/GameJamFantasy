using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public float attackPower;
    public float cooldown;
    public float criticalChance;

    private float lastAttackTime;

    private void Start()
    {
        lastAttackTime = -cooldown; // Set initial value to allow immediate attack
    }

    private void Update()
    {
        // For testing purposes, you can call Attack() when a certain input (e.g., mouse click) is detected.
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (Time.time - lastAttackTime > cooldown)
        {
            lastAttackTime = Time.time;

            // Raycast to detect enemies
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the EnemyHealth component
                EnemyHealth enemyHealthComponent = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealthComponent != null)
                {
                    float damage = CalculateDamage();
                    bool isCritical = IsCriticalHit();

                    // Call the takeDamageEnemy function on the EnemyHealth component
                    enemyHealthComponent.takeDamageEnemy(damage);
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
