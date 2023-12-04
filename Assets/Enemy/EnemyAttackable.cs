using UnityEngine;

public class EnemyAttackable : Attackable
{
    public delegate void EnemyTurnCompletedDelegate();
    public event EnemyTurnCompletedDelegate OnEnemyTurnCompleted;

    public float enemyAttackPower = 10f; // Sesuaikan kekuatan serangan sesuai kebutuhan
    private GameManager2 gameManager; // Referensi ke GameManager2
    private bool isEnemyTurn = false; // Flag untuk memeriksa apakah ini giliran musuh

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager2>(); // Temukan GameManager2 di dalam scene
    }

    // Fungsi untuk menetapkan flag giliran ketika ini adalah giliran musuh
    public void SetEnemyTurn()
    {
        isEnemyTurn = true;
    }

    public void EnemyAIAttack()
    {
        if (isEnemyTurn)
        {
            Debug.Log("EnemyAIAttack called.");

            AttackCharacter();

            Debug.Log("Enemy attacks the player!");

            // Setelah menyerang, naikkan event untuk menandakan selesainya giliran musuh
            OnEnemyTurnCompleted?.Invoke();

            // Setelah menyerang, reset flag giliran
            isEnemyTurn = false;
        }
        else
        {
            Debug.LogWarning("It's not the enemy's turn to attack.");
        }
    }

    private void AttackCharacter()
    {
        if (isEnemyTurn)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterHealth characterHealth = player.GetComponent<CharacterHealth>();
                if (characterHealth != null)
                {
                    // Lakukan serangan pada karakter
                    characterHealth.TakeDamageCharacter(enemyAttackPower);
                }
                else
                {
                    Debug.LogWarning("Player is missing CharacterHealth component.");
                }
            }
            else
            {
                Debug.LogWarning("Player not found.");
            }
        }
    }
}
