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
        List<Entity> allEntities = characters.Concat(enemies).OrderByDescending(entity => entity.power).ToList();

        sortedEntities = allEntities.ToList();

        foreach (Entity entity in sortedEntities)
        {
            HandleEntityTurn(entity);
        }

        Debug.Log($"Round {currentEntityIndex + 1} - Movement Order:");

        foreach (Entity entity in sortedEntities.Skip(1).Where(characters.Contains))
        {
            ActivateUI(entity.uiElement, false);
        }

        SetTurnForCurrentEntity();
    }

    void HandleEntityTurn(Entity entity)
    {
        GameObject entityObject = entity.gameObject;
        bool isActiveUI = entity == sortedEntities.First();
        ActivateUI(entity.uiElement, isActiveUI);

        if (characters.Contains(entity))
        {
            HandleCharacterTurn(entity);
        }
        else if (entity.attackableComponent is EnemyAttackable enemyAttackable)
        {
            if (enemyAttackable.IsEnemyTurn)
            {
                HandleEnemyTurn(entity, enemyAttackable);
            }
        }

        entityObject.AddComponent<ClickHandler>();
        entityObject.GetComponent<Attackable>()?.Attack();
    }

    void HandleCharacterTurn(Entity entity)
    {
        entity.attackableComponent?.StartCharacterTurn();
    }

    void HandleEnemyTurn(Entity entity, EnemyAttackable enemyAttackable)
    {
        if (entity.attackableComponent != null)
        {
            enemyAttackable.SetEnemyTurn();
            enemyAttackable.OnEnemyTurnCompleted += AdvanceToNextLowestPowerEntity;
            enemyAttackable.EnemyAIAttack();
        }
    }

    void SetTurnForCurrentEntity()
    {
        Entity currentEntity = sortedEntities[currentEntityIndex];
        if (enemies.Contains(currentEntity) && currentEntity.attackableComponent is EnemyAttackable enemyAttackable)
        {
            enemyAttackable.SetEnemyTurn();
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
            HandleCharacterTurn(nextEntity);
        }
        else if (enemies.Contains(nextEntity) && nextEntity.attackableComponent is EnemyAttackable enemyAttackable)
        {
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
        uiElement?.SetActive(isActive);
    }
}
