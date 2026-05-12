using UnityEngine;


public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject player; // Посилання на гравця
    [SerializeField] private DialogueController dialogueController; // Посилання на контролер діалогу

    public float dialogueDistance = 3f; // Максимальна відстань для початку діалогу
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    void Start()
    {

    }

    public void StartDialogue() 
    { 
        isDialogueActive = true;
        Debug.Log("Dialogue started with " + gameObject.name);
        // Тут можна додати код для запуску діалогу, наприклад, виклик DialogueController
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