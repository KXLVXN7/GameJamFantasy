using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHpEnemy = 100f;
    private float currHealthEnemy;
    private bool isDead = false;

    private void Start()
    {
        currHealthEnemy = maxHpEnemy;
    }

    public void TakeDamageEnemy(float damage, bool isCritical = false)
    {
        if (!isDead)
        {
            if (isCritical)
            {
                damage *= 2.0f; // Double the damage for critical hits
            }

            currHealthEnemy -= damage;
            currHealthEnemy = Mathf.Clamp(currHealthEnemy, 0, maxHpEnemy);
            UpdateHealthBar();

            if (currHealthEnemy <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHealthBar()
    {
        // Implement logic to update health bar UI
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Enemy mati!");
            // Additional logic for death, such as playing death animation
            //anim.SetBool("playerDeath", true);
            Destroy(gameObject); // Example: destroy the enemy object
        }
    }
}
