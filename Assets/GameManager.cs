using System.Collections;
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
        public Attackable attackableComponent;
    }

    public List<Entity> characters;
    public List<Entity> enemies;

    private List<Entity> sortedEntities;
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

            if (characters.Contains(entity))
            {
                // Character's turn
                if (entity.attackableComponent != null)
                {
                    entity.attackableComponent.StartCharacterTurn();
                }
            }
            else
            {
                // Enemy's turn
                if (entity.attackableComponent != null && entity.attackableComponent is EnemyAttackable)
                {
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

        Debug.Log($"Round {currentEntityIndex + 1} - Movement Order:");

        foreach (Entity entity in sortedEntities.Skip(1))
        {
            if (characters.Contains(entity))
            {
                ActivateUI(entity.uiElement, false);
            }
        }

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

        Entity currentEntity = sortedEntities[currentEntityIndex];
        ActivateUI(currentEntity.uiElement, false);

        currentEntityIndex = (currentEntityIndex + 1) % sortedEntities.Count;

        Entity nextEntity = sortedEntities[currentEntityIndex];
        StartCoroutine(AdvanceToNextEntityWithDelay(nextEntity));
    }

    private IEnumerator AdvanceToNextEntityWithDelay(Entity nextEntity)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay as needed

        ActivateUI(nextEntity.uiElement, true);
        Debug.Log($"Next entity with highest power: {nextEntity.name}");

        if (characters.Contains(nextEntity))
        {
            if (nextEntity.attackableComponent != null)
            {
                nextEntity.attackableComponent.StartCharacterTurn();
            }
        }
        else if (enemies.Contains(nextEntity) && nextEntity.attackableComponent is EnemyAttackable)
        {
            EnemyAttackable enemyAttackable = (EnemyAttackable)nextEntity.attackableComponent;
            enemyAttackable.SetEnemyTurn();
            enemyAttackable.EnemyAIAttack();

            if (characters.Contains(nextEntity))
            {
                ShowCharacterUITurn(nextEntity.uiElement);
            }
        }
    }

    void ShowCharacterUITurn(GameObject uiElement)
    {
        uiElement.SetActive(true);
    }

    void ActivateUI(GameObject uiElement, bool isActive)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(isActive);
        }
    }
}
