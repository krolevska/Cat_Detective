using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueController dialogueController;

    private bool isPlayerInRange = false;

    public void Interact()
    {
        StartDialogue();
    }
    public string InteractionPrompt => "Press E to talk to " + gameObject.name;
    public void StartDialogue() 
    {
        if (!isPlayerInRange) { return; }
        Debug.Log("Dialogue started with " + gameObject.name);
        dialogueController.StartDialogue();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}