using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float maxHp = 100;
    [SerializeField] float currHealthChar = 100;

    [SerializeField] private Image HPBar;
    private bool isDead = false;

    private void Start()
    {
        currHealthChar = maxHp;
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        
        currHealthChar += amount;
        Debug.Log("Character Semua menerima Heal sebanyak : " +currHealthChar );
    }

    private void UpdateHealthBar()
    {
        HPBar.fillAmount = currHealthChar / maxHp;
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.35f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void takeDamageCharacter(float damage)
    {
        if (!isDead)
        {
            currHealthChar -= damage;
            currHealthChar = Mathf.Clamp(currHealthChar, 0, maxHp);
            UpdateHealthBar();
            StartCoroutine(VisualIndicator(Color.red));
            if (currHealthChar <= 0)
            {
                Debug.Log("Tidak ada darah lagi");
                Die();
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Character mati!");
/*            anim.SetBool("playerDeath", true);*/
        }
    }
}
