using UnityEngine;
[CreateAssetMenu(fileName = "Clue_", menuName = "Clues", order = 1)]

public class ClueDataSO : ScriptableObject
{
    public string clueId;
    public string clueName;
    public string clueDescription;
}
