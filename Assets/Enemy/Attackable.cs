using UnityEngine;

public class Attackable : MonoBehaviour
{
    private GameManager.Entity entityInfo;

    public void SetEntity(GameManager.Entity entity)
    {
        entityInfo = entity;
    }

    public void Attack()
    {
        /*Debug.Log($"{entityInfo.name} melakukan serangan dengan kekuatan {entityInfo.power}!");*/
        // Lakukan logika serangan atau apapun yang sesuai untuk permainan Anda
    }
}
