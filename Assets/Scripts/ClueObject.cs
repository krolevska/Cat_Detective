using UnityEngine;

public class ClueObject : MonoBehaviour//, IInteractable
{
    [SerializeField] public ClueDataSO clueData;

    
    public void Interact()
    {

        AddClue();
    }

    private void AddClue() => ClueManager.Instance.AddClue(clueData);
}
