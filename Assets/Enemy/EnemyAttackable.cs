using UnityEngine;

public class EnemyAttackable : Attackable
{
    public float enemyAttackPower = 10f; // Adjust the attack power as needed
    private GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
    }

    public void EnemyAIAttack()
    {
        // Implement AI logic for enemy attacks here
        // For simplicity, let's assume the enemy attacks the player every time EnemyAIAttack is called
        AttackCharacter();

        // After attacking, advance to the next entity in GameManager
        gameManager.AdvanceToNextLowestPowerEntity();
    }

    private void AttackCharacter()
    {
        // Check if there is a character to attack
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the Attackable component from the character
            CharacterHealth characterHealth = player.GetComponent<CharacterHealth>();
            if (characterHealth != null)
            {
                // Perform the attack on the character
                characterHealth.TakeDamageCharacter(enemyAttackPower);

                // Optionally, you can add effects or animations for the enemy attack
                // ...

                Debug.Log("Enemy attacks the player!");
            }
            else
            {
                Debug.LogWarning("Player is missing Attackable component.");
            }
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }
    }
}
