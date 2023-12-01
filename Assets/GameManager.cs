using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Entity
    {
        public string name;
        public int power;
        public GameObject gameObject;
        public GameObject uiElement;
        public Attackable attackableComponent; // Add a reference to the Attackable component
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

        sortedEntities = allEntities.OrderByDescending(entity => entity.power).ToList();

        foreach (Entity entity in sortedEntities)
        {
            GameObject entityObject = entity.gameObject;

            bool isActiveUI = entity == sortedEntities.First();
            ActivateUI(entity.uiElement, isActiveUI);

            // Perform actions based on entity type (character or enemy)
            if (characters.Contains(entity))
            {
                // ... (rest of the code for characters)
            }
            else
            {
                // ... (rest of the code for enemies)
                if (entity.attackableComponent != null && entity.attackableComponent is EnemyAttackable)
                {
                    // If it's an enemy, call the AI attack method
                    ((EnemyAttackable)entity.attackableComponent).EnemyAIAttack();
                }
            }

            entityObject.AddComponent<ClickHandler>();
            Attackable attackableComponent = entityObject.GetComponent<Attackable>();
            if (attackableComponent != null)
            {
                attackableComponent.Attack();
            }
        }

        Debug.Log($"Round {currentRound} - Movement Order:");

        foreach (Entity entity in sortedEntities.Skip(1))
        {
            if (characters.Contains(entity))
            {
                ActivateUI(entity.uiElement, false);
            }
        }

        currentRound++;
        SetTurnForCurrentEntity();
    }

    void SetTurnForCurrentEntity()
    {
        Entity currentEntity = sortedEntities[currentEntityIndex];
        if (enemies.Contains(currentEntity) && currentEntity.attackableComponent is EnemyAttackable)
        {
            ((EnemyAttackable)currentEntity.attackableComponent).SetEnemyTurn();
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

        // Perform actions based on entity type (character or enemy)
        if (enemies.Contains(nextEntity) && nextEntity.attackableComponent is EnemyAttackable)
        {
            ((EnemyAttackable)nextEntity.attackableComponent).EnemyAIAttack();
        }
    }

    void ActivateUI(GameObject uiElement, bool isActive)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(isActive);
        }
    }
}
