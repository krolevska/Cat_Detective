using UnityEngine;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    public List<ClueObject> clueObjects;
    public List<ClueDataSO> availableClues;


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
        if (clueObjects.Exists(c => c.clueData.clueId == clueId))
        {
            Debug.Log("Player already has clue: " + clueId);
            return true;
        }
        return false;
    }
    public bool AddClueById(string clueId)
    {
        bool canAdd = !HasClue(clueId);
        if (canAdd)
        {
            ClueDataSO clueData = availableClues.Find(c => c.clueId == clueId);
            if (clueData != null)
            {
                AddClue(clueData);
                return true;
            }
            else
            {
                Debug.LogWarning("Clue data not found for ID: " + clueId);
            }
        }

        return canAdd;
    }
    public void AddClue(ClueDataSO clue)
    {
        // Add the clue to the player's inventory or clue log
        if (!HasClue(clue.clueId))
        {
            clueObjects.Add(new ClueObject { clueData = clue });
            Debug.Log("Clue added");
        }

    }
}

public enum CaseState { NotStarted, InProgress, Resolved }
