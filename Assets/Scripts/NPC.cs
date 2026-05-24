using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string triggerId;
    [SerializeField] private NarrativeDirector narrativeDirector;

    public void Interact()
    {
        narrativeDirector.StartNarrativeInteraction(triggerId);
    }
    public string InteractionPrompt => "Press E to talk to " + gameObject.name;
}