using System;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

<<<<<<< Updated upstream
    public List<ClueObject> clueObjects;

=======
    [SerializeField] private List<ClueDataSO> collectedClues = new();
    [SerializeField] private List<ClueDataSO> availableClues;
>>>>>>> Stashed changes

    public event Action<ClueDataSO> OnClueAdded;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public bool HasClue(string clueId)
    {
        // Check if the player has the specified clue
        if (collectedClues.Exists(c => c.clueId == clueId))
        {
            Debug.Log("Player already has clue: " + clueId);
            return true;
        }
        return false;
    }

    public void AddClue(ClueDataSO clue)
    {
        // Add the clue to the player's inventory or clue log
        if (!HasClue(clue.clueId))
        {
            collectedClues.Add(clue);
            OnClueAdded?.Invoke(clue);
            Debug.Log("Clue added");
        }

    }
}

public enum CaseState { NotStarted, InProgress, Resolved }
