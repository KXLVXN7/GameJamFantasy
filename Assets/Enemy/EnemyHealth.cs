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

    public void takeDamageEnemy(float damage)
    {
        if (!isDead)
        {
            currHealthEnemy -= damage;
            currHealthEnemy = Mathf.Clamp(currHealthEnemy, 0, maxHpEnemy);
            UpdateHealthBar();
            /*StartCoroutine(VisualIndicator(Color.red));*/
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

/*    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.35f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }*/

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
