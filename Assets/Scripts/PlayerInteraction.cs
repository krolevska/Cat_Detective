using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerInteraction : MonoBehaviour
{
    #region Serialized Fields

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactableLayer; // Шар об'єктів для взаємодії
    [SerializeField] private LayerMask NPCInteraction; // Шар NPC
    [SerializeField] private float interactionRange = 3f; // Дальність взаємодії
    [SerializeField] private Transform playerCamera; // Посилання на камеру гравця
    [SerializeField] private InputActionReference interactAction; // Посилання на дію взаємодії

    #endregion

    #region Variables

    private Interactable currentInteractable; // Поточний об'єкт для взаємодії
    private NPC currentNPC; // Поточний NPC для взаємодії

    #endregion

    #region Build-in Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        // Перевірка на взаємодію з NPC
        if (collision.TryGetComponent<NPC>(out NPC npc))
        {

            //  npc = collision.GetComponent<NPC>();

            if (npc != null)
            {

                currentNPC = npc;
                return;
            }
        }
        // Перевірка на взаємодію з об'єктом

        else if (collision.TryGetComponent<Interactable>(out Interactable interactable))
        {
            Debug.Log("Collided with Interaction layer");

            interactable = collision.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                return;
            }
        }
        else
        {
            // Якщо немає об'єктів для взаємодії
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