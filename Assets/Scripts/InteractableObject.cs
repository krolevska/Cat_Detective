using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private NarrativeDirector narrativeDirector;
    [SerializeField] private string triggerId;
    [SerializeField] private string objectName;
    public void Interact()
    {
        narrativeDirector.StartNarrativeInteraction(triggerId);
    }
    public string InteractionPrompt => "Натисни E, щоб взаємодіяти з " + objectName;
}
