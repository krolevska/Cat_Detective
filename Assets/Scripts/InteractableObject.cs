using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        StartExploration();
    }
    public string InteractionPrompt => "Press E to explore " + gameObject.name;

    private void StartExploration()
    {
        Debug.Log("Exploring " + gameObject.name);
        // Implement exploration logic here (e.g., show item details, add to inventory, etc.)
    }
}
