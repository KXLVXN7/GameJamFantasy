using System.Collections.Generic;

public class EnemyAttackable : Attackable
{
    public void EnemyAIAttack()
    {
/*        Debug.Log($"{gameObject.name} is performing AI attack!");
*/    }

    public void SetEnemyTurn()
    {
/*        Debug.Log($"{gameObject.name} is now the active enemy.");
*/    }

    public void EnemyAttackCharacter(List<GameManager.Entity> characters)
    {
        if (characters != null && characters.Count > 0)
        {
            GameManager.Entity targetCharacter = characters[0];
/*            Debug.Log($"{gameObject.name} is attacking {targetCharacter.name}!");
*/            // Implement logic to perform the attack on the selected character
        }
        else
        {
/*            Debug.Log("No characters available for enemy attack.");
*/        }
    }
}
