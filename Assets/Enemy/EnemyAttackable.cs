using UnityEngine;

public class EnemyAttackable : Attackable
{
    public delegate void EnemyTurnCompletedDelegate();
    public event EnemyTurnCompletedDelegate OnEnemyTurnCompleted;

    public float enemyAttackPower = 10f; // Adjust the attack power as needed
    private GameManager gameManager; // Reference to the GameManager
    private bool isEnemyTurn = false; // Flag to check if it's the enemy's turn

    // Property to check if it's the enemy's turn
    public bool IsEnemyTurn => isEnemyTurn;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
    }

    // Function to set the turn flag when it's the enemy's turn
    public void SetEnemyTurn()
    {
        isEnemyTurn = true;
        // Optionally, you can add log or notification when the enemy's turn starts
        // Debug.Log($"{entityInfo.name}'s turn has started!");
    }

    public void EnemyAIAttack()
    {
        if (isEnemyTurn)
        {
            Debug.Log("EnemyAIAttack called.");

            AttackCharacter();

            // Optionally, you can add effects or animations for the enemy attack
            // ...

            Debug.Log("Enemy attacks the player!");

            // After attacking, raise the event to signal the completion of the enemy's turn
            OnEnemyTurnCompleted?.Invoke();

            // After attacking, reset the turn flag
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
                    // Perform the attack on the character
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
