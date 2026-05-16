using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        StartExploration();
    }

    private void StartExploration()
    {
        Debug.Log("Exploring " + gameObject.name);
        // Implement exploration logic here (e.g., show item details, add to inventory, etc.)
    }
}
