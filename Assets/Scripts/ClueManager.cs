using UnityEngine;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour
{
    public ClueManager Instance { get; private set; }

    public List<ClueObject> ClueObjects;
    

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
        return false;
    }

    public void AddClue(ClueDataSO clue)
    {
        // Add the clue to the player's inventory or clue log
    }
}

public enum CaseState { NotStarted, InProgress, Resolved }
