using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    #region Serialized Fields

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask NPCInteraction;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject interactionPrompt;
    public InputActionReference InteractAction { get => interactAction; set => interactAction = value; }

    #endregion

    #region Variables

    private Item currentInteractable;
    private NPC currentNPC;

    #endregion

    #region Build-in Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        // Check for NPC interaction first
        if (collision.TryGetComponent<NPC>(out NPC npc))
        {

            npc = collision.GetComponent<NPC>();
            if (npc != null)
            {
                interactionPrompt.SetActive(true);
                interactionPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "Press E to talk to " + npc.gameObject.name;
                currentNPC = npc;
                return;
            }
        }
        // Check for Interactable objects if no NPC was found

        else if (collision.TryGetComponent<Item>(out Item interactable))
        {
            Debug.Log("Collided with Interaction layer");

            interactable = collision.GetComponent<Item>();

            if (interactable != null)
            {
                interactionPrompt.SetActive(true);
                interactionPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "Press E to interact with " + collision.gameObject.name;
                currentInteractable = interactable;
                return;
            }
        }
        else
        {
            // If the collided object is neither an NPC nor an Interactable, clear the current references
            currentInteractable = null;
            currentNPC = null;
            interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionPrompt.SetActive(false);
        // Clear references when exiting the trigger
        if (collision.TryGetComponent<NPC>(out NPC npc))
        {
            if (currentNPC == npc)
            {
                currentNPC = null;
            }
        }
        else if (collision.TryGetComponent<Item>(out Item interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable = null;
            }
        }
    }


    private void OnEnable()
    {
        interactAction.action.started += Interact;
    }

    private void OnDisable()
    {
        interactAction.action.started -= Interact;
    }
    #endregion

    #region Custom Methods

    private void Interact(InputAction.CallbackContext obj)
    {

        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
        else if (currentNPC != null)
        {
            currentNPC.StartDialogue();
        }
    }
    #endregion
}