using UnityEngine;

public class ClueObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ClueDataSO clueData;

    
    public void Interact()
    {

        AddClue();
    }

    private void AddClue() => ClueManager.Instance.AddClue(clueData);
}
