using UnityEngine;
using System;

public class Attackable : MonoBehaviour
{
    private GameManager.Entity entityInfo;

    // Event to notify completion of an attack
    public event Action OnAttackCompleted;

    public void SetEntity(GameManager.Entity entity)
    {
        entityInfo = entity;
    }

    public void Attack()
    {
        // Debug.Log($"{entityInfo.name} melakukan serangan dengan kekuatan {entityInfo.power}!");
        // Lakukan logika serangan atau apapun yang sesuai untuk permainan Anda

        // Notify that the attack is completed
     /*   OnAttackCompleted?.Invoke();*/
    }

    // Add this method for starting a character's turn
    public void StartCharacterTurn()
    {
        // Implement logic for starting a character's turn
        /*   // For example, display a panel with character actions, skills, etc.
           Debug.Log($"{entityInfo.name}'s turn has started!");*/
    }
}
