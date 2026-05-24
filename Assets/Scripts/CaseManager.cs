using UnityEngine;

public enum CaseState
{
    NotStarted,
    InProgress,
    ReadyToConclude,
    Resolved
}

public class CaseManager : MonoBehaviour
{

    [SerializeField] private CaseDataSO activeCase;
    [SerializeField] private ClueManager clueManager;
    

    public static CaseManager Instance { get; private set; }
    public void Awake()
    {
        if ( Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void StartCase()
    {
        if (activeCase == null)
        {
            Debug.LogWarning("No active case assigned to CaseManager.");
            return;
        }
        activeCase.caseState = CaseState.InProgress;
        clueManager.OnClueAdded += CheckCaseProgress;
    }

    private void CheckCaseProgress(ClueDataSO addedClue)
    {
        if (activeCase.caseState != CaseState.InProgress) return;
        if (HasAllRequiredClues())
        {
            activeCase.caseState = CaseState.ReadyToConclude;
            Debug.Log("All required clues collected. Case is ready to conclude.");
        }
    }

    private bool HasAllRequiredClues()
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
}
