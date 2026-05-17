using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    #region Serialized Fields

    [Header("Interaction Settings")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject interactionPrompt;


    #endregion

    #region Variables

    private IInteractable currentInteractable;

    #endregion

    #region Build-in Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        if (collision.TryGetComponent<NPC>(out NPC npc))
        {
            if (npc != null)
            {
                currentInteractable = npc;
                ShowPrompt(true, currentInteractable.InteractionPrompt);
                return;
            }
        }

        else if (collision.TryGetComponent<InteractableObject>(out InteractableObject interactable))
        {
            Debug.Log("Collided with Interaction layer");
            if (interactable != null)
            {
                currentInteractable = interactable;
                ShowPrompt(true, currentInteractable.InteractionPrompt);
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<NPC>(out NPC npc))
        {
            if (currentInteractable == npc)
            {
                interactionPrompt.SetActive(false);
                currentInteractable = null;
            }
        }
        else if (collision.TryGetComponent<InteractableObject>(out InteractableObject interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable = null;
                interactionPrompt.SetActive(false);
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
        currentInteractable?.Interact();
    }

    public void SetInteractionEnabled(bool enabled)
    {
        if (enabled)
            interactAction.action.Enable();
        else
            interactAction.action.Disable();
    }

    private void ShowPrompt(bool enabled, string message = "")
    {
        interactionPrompt.SetActive(enabled);
        if (enabled)
        {
            interactionPrompt.GetComponentInChildren<TextMeshProUGUI>().text = message;
        }
    }
    #endregion
}