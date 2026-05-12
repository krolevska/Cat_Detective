using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueController dialogueController;

    public float dialogueDistance = 3f;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    public void Interact()
    {
        StartDialogue();
    }

    public void StartDialogue() 
    {
        if (!isPlayerInRange) { return; }
        isDialogueActive = true;
        Debug.Log("Dialogue started with " + gameObject.name);
        dialogueController.StartDialogue();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
        }
    }
}