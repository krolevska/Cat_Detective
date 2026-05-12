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

    #endregion

    #region Variables

    private IInteractable currentInteractable;
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

                currentNPC = npc;
                return;
            }
        }

        // Check for Interactable objects if no NPC was found

        else if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            Debug.Log("Collided with Interaction layer");

            interactable = collision.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                return;
            }
        }
        else
        {
            // If the collided object is neither an NPC nor an Interactable, clear the current references
            currentInteractable = null;
            currentNPC = null;
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