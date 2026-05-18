using Ink.Parsed;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeDirector : MonoBehaviour
{
    /*
     * Підключити показ тексту через DialogueUI
     * Додати корисний debug message, якщо trigger ID не знайдено
     * Перевірити, що різні тригери запускають різні фрагменти Ink story
     */

    [SerializeField] private TextAsset sceneInkJSON = null;

    public event Action<string> NarrativeTagReceived;
    private Ink.Runtime.Story story;
    private List<string> currentTags;

    public static NarrativeDirector Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        story = new Ink.Runtime.Story(sceneInkJSON.text);

        story.onError += (message, type) =>
        {
            if (type == Ink.ErrorType.Warning)
                Debug.LogWarning($"Ink warning: {message}");
            else
                Debug.LogError($"Ink error: {message}");
        };


        BindExternalFunctions();
    }

    public void PlayKnot(string knotName)
    {
        if (string.IsNullOrWhiteSpace(knotName))
        {
            Debug.LogWarning("Ink knot name is empty.");
            return;
        }

        Debug.Log("Triggered: " + knotName);

        if (story.KnotContainerWithName(knotName) != null)
        {
            story.ChoosePathString(knotName);
            ContinueStory();
        }
        else
        {
            Debug.LogWarning("Knot not found in story: " + knotName);
        }
    }

    private void ContinueStory()
    {
        while (story.canContinue)
        {
            currentTags = ProcessTags(story.currentTags);
            string line = story.Continue().Trim();
            Debug.Log(line);
            DialogueUI.Instance.ShowText(line);
        }
    }

    private void BindExternalFunctions()
    {

    }
    private List<string> ProcessTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            NarrativeTagReceived?.Invoke(tag.Trim());
        }
        return tags;
    }
    public string SaveInkState()
    {
        return story.state.ToJson();
    }

    public void LoadInkState(string json)
    {
        story.state.LoadJson(json);
    }
}
