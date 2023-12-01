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

        List<Entity> sortedEntities = allEntities.OrderByDescending(entity => entity.power).ThenBy(entity => allEntities.IndexOf(entity)).ToList();

        foreach (Entity entity in sortedEntities)
        {
            GameObject entityObject = entity.gameObject;

            if (characters.Contains(entity))
            {
                bool isActiveUI = entity == sortedEntities.First();
                ActivateUI(entity.uiElement, isActiveUI);
            }

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
        // Set the turn for the first enemy
        if (enemies.Count > 0)
        {
            EnemyAttackable firstEnemy = enemies[0].attackableComponent as EnemyAttackable;
            if (firstEnemy != null)
            {
                firstEnemy.SetEnemyTurn();
            }
        }

        // ... (remaining code)
    }

    void ActivateUI(GameObject uiElement, bool isActive)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(isActive);
        }
    }

    public void AdvanceToNextLowestPowerEntity()
    {
        List<Entity> allEntities = new List<Entity>(characters);
        allEntities.AddRange(enemies);

        List<Entity> sortedEntities = allEntities.OrderByDescending(entity => entity.power).ThenBy(entity => allEntities.IndexOf(entity)).ToList();

        if (currentEntityIndex < sortedEntities.Count - 1)
        {
            Entity nextEntity = sortedEntities[currentEntityIndex + 1];

            currentEntityIndex++;

            Debug.Log($"Next entity with highest power: {nextEntity.name}");

            if (characters.Contains(sortedEntities[currentEntityIndex - 1]))
            {
                ActivateUI(sortedEntities[currentEntityIndex - 1].uiElement, false);
            }

            if (characters.Contains(nextEntity))
            {
                ActivateUI(nextEntity.uiElement, true);
            }
            else
            {
                // If it's an enemy, call the AI attack method
                if (nextEntity.attackableComponent != null && nextEntity.attackableComponent is EnemyAttackable)
                {
                    ((EnemyAttackable)nextEntity.attackableComponent).EnemyAIAttack();
                }
            }
        }
        else
        {
            currentEntityIndex = 0;
        }
    }
}
