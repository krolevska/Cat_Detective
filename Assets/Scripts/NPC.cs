using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string triggerId;
    [SerializeField] private string npcName;
    [SerializeField] private NarrativeDirector narrativeDirector;

    public void Interact()
    {
        narrativeDirector.StartNarrativeInteraction(triggerId);
    }
    public string InteractionPrompt => "Натисни E, щоб поговорити з " + npcName;
}