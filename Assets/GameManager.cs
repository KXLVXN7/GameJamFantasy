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
        public GameObject gameObject; // Use GameObject instead of prefab
        public GameObject uiElement;
    }

    public List<Entity> characters;
    public List<Entity> enemies;

    private int currentRound = 1;

    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        // Combine characters and enemies into one list
        List<Entity> allEntities = new List<Entity>(characters);
        allEntities.AddRange(enemies);

        // Sort the list based on power in descending order
        List<Entity> sortedEntities = allEntities.OrderByDescending(entity => entity.power).ToList();

        // List to store movement order
        List<string> movementOrder = new List<string>();

        // Loop to determine movement order
        foreach (Entity entity in sortedEntities)
        {
            GameObject entityObject = entity.gameObject; // Use the provided game object

            // Do something with the object, such as adding it to a list or moving it
            // ...

            if (characters.Contains(entity))
            {
                movementOrder.Add($"Character {characters.IndexOf(entity) + 1}");

                // Call the ActivateUI function if the object has a UI element to be activated
                ActivateUI(entity.uiElement);
            }
            else
            {
                movementOrder.Add($"Enemy {enemies.IndexOf(entity) + 1}");
            }

            // Add the ClickHandler component to respond to clicks
            entityObject.AddComponent<ClickHandler>();

            // Call the Attack function if the object has an Attackable component
            Attackable attackableComponent = entityObject.GetComponent<Attackable>();
            if (attackableComponent != null)
            {
                attackableComponent.Attack();
            }
        }

        // Display the movement order in the console
        Debug.Log($"Round {currentRound} - Movement Order:");
        foreach (string entity in movementOrder)
        {
            Debug.Log(entity);
        }

        // End of round, proceed to the next round (in this example, add 1 to the current round)
        currentRound++;
        // Add logic or call the next function to continue the game
    }

    void ActivateUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }
}
