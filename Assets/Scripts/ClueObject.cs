using UnityEngine;

public class ClueObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ClueDataSO clueData;

    public void Interact()
    {
        // Display the clue information to the player
        // ClueUI.Instance.ShowClue(clueData);
        // if ()
        AddClue(clueData);
    }

    private void AddClue(ClueDataSO clue)
    {
        // Add the clue to the player's inventory or clue log
        // ClueManager.Instance.AddClue(clue);
    }
}
