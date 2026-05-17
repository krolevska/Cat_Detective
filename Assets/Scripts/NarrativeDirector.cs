using UnityEngine;
using Ink.Runtime;

public class NarrativeDirector : MonoBehaviour
{
    /*
     * Підготувати клас до подальшого підключення UI і переходу в Ink knots
     */
    [SerializeField] private TextAsset sceneInkJSON = null;

    private Story story;

    public static NarrativeDirector Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        story = new Story(sceneInkJSON.text);
    }

    public void Trigger(string triggerId)
    {
        Debug.Log("Triggered: " + triggerId);
        /*
        if (story.variablesState.Contains(triggerId))
        {
            story.variablesState[triggerId] = true;
        }
        else
        {
            Debug.LogWarning("Trigger not found in story: " + triggerId);
        }
        */
    }
}
