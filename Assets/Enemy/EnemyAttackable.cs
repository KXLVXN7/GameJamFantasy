using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackable : Attackable   
{
    public void EnemyAIAttack(List<GameManager.Entity> characters)
    {
        if (characters != null && characters.Count > 0 && gameObject != null)
        {
            GameManager.Entity targetCharacter = characters[0];

            Debug.Log($"{gameObject.name} is attacking {targetCharacter.name}!");
            AttackCharacter(targetCharacter);

            if (IsCharacterDefeated(targetCharacter))
            {
                Debug.Log($"{targetCharacter.name} is defeated!");

                GameManager.Entity nextTarget = FindNextTarget(characters);

                if (nextTarget != null)
                {
                    Debug.Log($"{gameObject.name} is now attacking {nextTarget.name}!");
                    AttackCharacter(nextTarget);
                }
                else
                {
                    Debug.Log($"{gameObject.name} has no more targets.");
                    // Implement logic when there are no more targets
                }
            }
        }
        else
        {
            Debug.LogWarning("No characters available for enemy attack or gameObject is null.");
        }
    }

    private void AttackCharacter(GameManager.Entity targetCharacter)
    {
        if (gameObject != null && targetCharacter != null)
        {
            Debug.Log($"{gameObject.name} is attacking {targetCharacter.name}!");
            // Implement the logic for attacking the target character
        }
    }

    private bool IsCharacterDefeated(GameManager.Entity character)
    {
        // Implement logic to check if the character is defeated
        // For example, check health, status, etc.
        return true; // Change this based on your actual logic
    }

    private GameManager.Entity FindNextTarget(List<GameManager.Entity> characters)
    {
        if (characters != null && characters.Count > 0)
        {
            IEnumerable<GameManager.Entity> characterEnumerable = characters;
            GameManager.Entity nextTarget = characterEnumerable.OrderBy(c => c.power).FirstOrDefault();
            return nextTarget;
        }

        return null;
    }

    // Add this method to set the turn for the enemy
    public void SetEnemyTurn()
    {
        if (gameObject != null)
        {
            // Implementation to set the turn for the enemy
            Debug.Log($"{gameObject.name} is now the active enemy.");
        }
    }
}
