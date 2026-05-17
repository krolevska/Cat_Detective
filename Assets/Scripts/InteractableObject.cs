using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private NarrativeDirector narrativeDirector;
    [SerializeField] private string triggerId;
    public void Interact()
    {
        narrativeDirector.Trigger(triggerId);
    }
    public string InteractionPrompt => "Press E to explore " + gameObject.name;
}
