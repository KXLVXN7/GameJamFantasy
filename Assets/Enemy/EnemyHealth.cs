using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float maxHpEnemy = 100;
    private float currHealthEnemy = 100;

    [SerializeField] private Image HPBar;
    private bool isDead = false;

    private void Start()
    {
        currHealthEnemy = maxHpEnemy;
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {

        currHealthEnemy += amount;
        Debug.Log("Enemy Semua menerima Heal sebanyak : " + currHealthEnemy);
    }

    private void UpdateHealthBar()
    {
        HPBar.fillAmount = currHealthEnemy / maxHpEnemy;
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.35f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void takeDamageEnemy(float damage)
    {
        if (!isDead)
        {
            currHealthEnemy -= damage;
            currHealthEnemy = Mathf.Clamp(currHealthEnemy, 0, maxHpEnemy);
            UpdateHealthBar();
            StartCoroutine(VisualIndicator(Color.red));
            if (currHealthEnemy <= 0)
            {
                Debug.Log("Enemy Tidak ada darah lagi");
                Die();
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Enemy mati!");
/*            anim.SetBool("playerDeath", true);*/
        }
    }
}
