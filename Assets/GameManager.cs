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
        public bool hasMoved;
        public bool canAttack; // Tambahkan variabel canAttack
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
        sortedEntities = characters.OrderByDescending(entity => entity.power)
                         .Concat(enemies.OrderByDescending(entity => entity.power))
                         .ToList();
        Debug.Log($"Round {currentEntityIndex + 1} - Movement Order:");

        // Reset canAttack flag for all characters
        foreach (Entity character in characters)
        {
            character.canAttack = false;
        }

        // Handle the turn for the first entity
        HandleEntityTurn(sortedEntities.First());
    }

    void HandleEntityTurn(Entity entity)
    {
        bool isActiveUI = entity == sortedEntities.First();

        if (characters.Contains(entity) && entity != characters[0])
        {
            ShowCharacterUITurn(entity.uiElement);
            ActivateUI(entity.uiElement, isActiveUI);

            // Check if it's the player character's turn and canAttack is true
            if (entity == characters[0] && entity.canAttack)
            {
                // Player character's turn to attack
                // Implement your player input logic for attack here
                Debug.Log($"{entity.name} attacks!");

                // TODO: Implement the logic for player character's attack

                entity.hasMoved = true; // Mark as moved
                entity.canAttack = false; // Reset canAttack flag
                AdvanceToNextLowestPowerEntity(); // Move to the next entity
            }
        }
        else if (entity.attackableComponent != null && entity.canAttack)
        {
            HandleEnemyTurn(entity, entity.attackableComponent);
        }
    }


    void HandleEnemyTurn(Entity entity, Attackable attackableComponent)
    {
        if (!entity.hasMoved)
        {
            // Simulate enemy attack on the character with the highest power
            Entity targetCharacter = characters.OrderByDescending(c => c.power).First();
            Debug.Log($"{entity.name} attacks {targetCharacter.name}");

            // TODO: Implement the logic for enemy attack

            entity.hasMoved = true;
            AdvanceToNextLowestPowerEntity();
        }
        else
        {
            AdvanceToNextLowestPowerEntity();
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

        // Reset the hasMoved and canAttack flags for all entities
        foreach (Entity entity in sortedEntities)
        {
            entity.hasMoved = false;
            entity.canAttack = false;
        }

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

    // Public method to allow player character to trigger attack
    public void TriggerPlayerAttack()
    {
        // Set canAttack to true for the player character
        if (characters.Count > 0)
        {
            characters[0].canAttack = true;
        }
    }
}
