using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]public float maxHp = 100;
    [SerializeField]public float currHealthChar = 100;

    [SerializeField] private Image HPBar;
    private bool isDead = false;

    private void Start()
    {
        currHealthChar = maxHp;
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        if (!isDead)
        {
            currHealthChar += amount;
            currHealthChar = Mathf.Clamp(currHealthChar, 0, maxHp);
            UpdateHealthBar();
            Debug.Log($"Character menerima Heal sebanyak: {amount}");
        }
    }

    public void checkCurrHP()
    {
        currHealthChar = currHealthChar;
    }

    private void UpdateHealthBar()
    {
        if (HPBar != null)
        {
            HPBar.fillAmount = currHealthChar / maxHp;
        }
    }

    private IEnumerator VisualIndicator(Color color)
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.35f);
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void TakeDamageCharacter(float damage)
    {
        if (!isDead)
        {
            currHealthChar -= damage;
            currHealthChar = Mathf.Clamp(currHealthChar, 0, maxHp);
            UpdateHealthBar();
            StartCoroutine(VisualIndicator(Color.red));
            if (currHealthChar <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Character mati!");
        }
    }
}
