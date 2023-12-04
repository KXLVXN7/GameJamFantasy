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
        // Separate sorting for characters and enemies
        sortedEntities = characters.OrderByDescending(entity => entity == characters[0] ? int.MinValue : entity.power)
                         .Concat(enemies.OrderByDescending(entity => entity.power))
                         .ToList();
        Debug.Log($"Round {currentEntityIndex + 1} - Movement Order:");
        // Handle the turn for the first entity
        HandleEntityTurn(sortedEntities.First());
    }

    void HandleEntityTurn(Entity entity)
    {
        GameObject entityObject = entity.gameObject;
        bool isActiveUI = entity == sortedEntities.First();

        if (characters.Contains(entity) && entity != characters[0])
        {
            ShowCharacterUITurn(entity.uiElement);
            ActivateUI(entity.uiElement, isActiveUI);
        }
        else if (entity.attackableComponent != null)
        {
            HandleEnemyTurn(entity, entity.attackableComponent);
        }

        entityObject.AddComponent<ClickHandler>();
        entityObject.GetComponent<Attackable>()?.Attack();
    }

    void HandleEnemyTurn(Entity entity, Attackable attackableComponent)
    {
        // Simulate enemy attack on a random character (adjust as needed)
        int randomCharacterIndex = Random.Range(0, characters.Count);
        Entity targetCharacter = characters[randomCharacterIndex];

        Debug.Log($"{entity.name} attacks {targetCharacter.name}");

        // TODO: Implement the logic for enemy attack

        AdvanceToNextLowestPowerEntity();
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
        Debug.Log($"Current entity with highest power: {currentEntity.name}");

        currentEntityIndex = (currentEntityIndex + 1) % sortedEntities.Count;

        // Check if all entities have taken their turns in the current round
        if (currentEntityIndex == 0)
        {
            // If all entities have taken their turns, start the next round
            StartCoroutine(StartNextRoundWithDelay());
        }
        else
        {
            // Otherwise, advance to the next entity
            Entity nextEntity = sortedEntities[currentEntityIndex];
            StartCoroutine(AdvanceToNextEntityWithDelay(nextEntity));
        }
    }

    private IEnumerator StartNextRoundWithDelay()
    {
        yield return new WaitForSeconds(1f); // Adjust delay if necessary

        // Start the next round
        StartRound();
    }

    private IEnumerator AdvanceToNextEntityWithDelay(Entity nextEntity)
    {
        yield return new WaitForSeconds(1f); // Adjust delay if necessary

        HandleEntityTurn(nextEntity);
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
