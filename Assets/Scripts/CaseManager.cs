using UnityEngine;
using System.Collections.Generic;
using System;

public enum CaseState
{
    NotStarted,
    InProgress,
    ReadyToConclude,
    Resolved
}

public class CaseManager : MonoBehaviour
{

    [SerializeField] private List<CaseDataSO> activeCases;
    [SerializeField] private List<CaseDataSO> possibleCases;
    [SerializeField] private ClueManager clueManager;

    public Action<string> caseResolved;
    public static CaseManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        clueManager.OnClueAdded += CheckCaseProgress;
    }

    public void AddCase(string caseID)
    {
        if (activeCases.Exists(c => c.caseId == caseID))
        {
            Debug.LogWarning("Case already active: " + caseID);
            return;
        }

        CaseDataSO newCase = possibleCases.Find(c => c.caseId == caseID);
        activeCases.Add(newCase);
    }

    private void CheckCaseProgress(ClueDataSO addedClue)
    {
        if (!activeCases.Exists(c => c.requiredClues.Contains(addedClue)) && !activeCases.Exists(c => c.optionalClues.Contains(addedClue))) return;

        foreach (var c in activeCases)
        {
            if (HasAllRequiredClues(c))
            {
                c.caseState = CaseState.ReadyToConclude;
                Debug.Log($"All required clues collected. Case '{c.caseName}' is ready to conclude.");
            }
        }
    }

    private bool HasAllRequiredClues(CaseDataSO activeCase)
    {
        foreach (ClueDataSO clue in activeCase.requiredClues)
        {
            if (!clueManager.HasClue(clue.clueId))
            {
                return false;
            }
        }
        return true;
    }

    public void ConcludeCase(string caseID)
    {
        CaseDataSO caseToConclude = activeCases.Find(c => c.caseId == caseID);
        if (caseToConclude != null && caseToConclude.caseState == CaseState.ReadyToConclude)
        {
            caseToConclude.caseState = CaseState.Resolved;
            caseResolved?.Invoke(caseID);
            Debug.Log($"Case '{caseToConclude.caseName}' has been resolved!");
            // Additional logic for concluding the case (e.g., rewards, narrative progression) can be added here
        }
        else
        {
            Debug.LogWarning($"Case '{caseID}' cannot be concluded. It may not be ready or does not exist.");
        }
    }
}
