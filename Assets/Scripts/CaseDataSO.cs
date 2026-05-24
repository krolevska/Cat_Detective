using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Case_", menuName ="Cases", order = 0)]
public class CaseDataSO : ScriptableObject
{
    public string caseId;
    public string caseName;
    public List<ClueDataSO> requiredClues;
    public List<ClueDataSO> optionalClues;

    public CaseState caseState = CaseState.NotStarted;
}
