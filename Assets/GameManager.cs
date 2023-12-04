using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Entity
    {
        public string name;
        public int power;
        public GameObject gameObject;
        public GameObject uiElement;
        public Attackable attackableComponent; // Reference to the Attackable component
    }

    public List<Entity> characters;
    public List<Entity> enemies;

    private List<Entity> sortedEntities;
    private int currentRound = 1;
    private int currentEntityIndex = 0;

    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        List<Entity> allEntities = new List<Entity>(characters);
        allEntities.AddRange(enemies);

        // Sort entities by power in descending order
        sortedEntities = allEntities.OrderByDescending(entity => entity.power).ToList();

        // Activate UI for the first entity and set up click handling
        foreach (Entity entity in sortedEntities)
        {
            ActivateUI(entity.uiElement, entity == sortedEntities.First());
            entity.gameObject.AddComponent<ClickHandler>();

            // Perform actions based on entity type (character or enemy)
            if (characters.Contains(entity))
            {
                // ... (rest of the code for characters)
            }
            else
            {
                // ... (rest of the code for enemies)
                if (entity.attackableComponent is EnemyAttackable enemyAttackable)
                {
                    enemyAttackable.EnemyAIAttack();
                }
            }
        }

        Debug.Log($"Round {currentRound} - Movement Order:");

        // Deactivate UI for all characters except the first one
        foreach (Entity entity in sortedEntities.Skip(1).Where(characters.Contains))
        {
            ActivateUI(entity.uiElement, false);
        }

        currentRound++;
        SetTurnForCurrentEntity();
    }

    void SetTurnForCurrentEntity()
    {
        if (currentEntityIndex >= 0 && currentEntityIndex < sortedEntities.Count)
        {
            Entity currentEntity = sortedEntities[currentEntityIndex];

            if (currentEntity.attackableComponent != null)
            {
                currentEntity.attackableComponent.Attack();

                // If it's an enemy, set its turn and perform AI attack
                if (currentEntity.attackableComponent is EnemyAttackable enemyAttackable)
                {
                    enemyAttackable.SetEnemyTurn();
                    enemyAttackable.EnemyAttackCharacter(characters);
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid currentEntityIndex");
        }
    }

    public void AdvanceToNextLowestPowerEntity()
    {
        if (sortedEntities == null || sortedEntities.Count == 0)
        {
            Debug.LogWarning("No entities to advance.");
            return;
        }

        // Deactivate UI for the current entity
        Entity currentEntity = sortedEntities[currentEntityIndex];
        ActivateUI(currentEntity.uiElement, false);

        // Move to the next entity in a round-robin fashion
        currentEntityIndex = (currentEntityIndex + 1) % sortedEntities.Count;

        // Activate UI for the next entity
        Entity nextEntity = sortedEntities[currentEntityIndex];
        ActivateUI(nextEntity.uiElement, true);

        Debug.Log($"Next entity with highest power: {nextEntity.name}");

        SetTurnForCurrentEntity();
    }

    void ActivateUI(GameObject uiElement, bool isActive)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(isActive);
        }
    }
}