using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    void OnMouseDown()
    {
        // Panggil fungsi Attack jika objek memiliki komponen Attackable
        Attackable attackableComponent = GetComponent<Attackable>();
        if (attackableComponent != null)
        {
            attackableComponent.Attack();
        }
    }
}
