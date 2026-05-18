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

    private Ink.Runtime.Story story;

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
            ProcessTags(story.currentTags);
            ProcessText(story.Continue().Trim());
        }
    }

    private void BindExternalFunctions()
    {
        story.BindExternalFunction("AddFact", (string factId) =>
        {
            Debug.Log($"New fact unlocked: {factId}");
            // FactJournal.Instance.Unlock(factId);
        });

        story.BindExternalFunction("UnlockConclusion", (string conclusionId) =>
        {
            Debug.Log($"Conclusion unlocked: {conclusionId}");
            // ConclusionSystem.Instance.Unlock(conclusionId);
        });

        story.BindExternalFunction("UpdateObjective", (string objectiveId) =>
        {
            Debug.Log($"Objective updated: {objectiveId}");
            // QuestLog.Instance.SetObjective(objectiveId);
        });

        story.BindExternalFunction("CompleteObjective", (string objectiveId) =>
        {
            Debug.Log($"Objective updated: {objectiveId}");
            // QuestLog.Instance.SetObjective(objectiveId);
        });
    }
    private void ProcessTags(List<string> tags)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            string[] parts = tags[i].Split(':');
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                PassTagsToUI(key, value);
            }
        }
    }

    private void PassTagsToUI(string key, string value)
    {
        switch (key)
        {
            case "style":
                DialogueUI.Instance.SetTextStyle(value);
                break;
            case "speaker":
                DialogueUI.Instance.SetSpeakerName(value);
                break;
            case "trigger":
                // Наприклад: TriggerManager.Instance.HandleTrigger(value);
                break;
            case "prompt":
                DialogueUI.Instance.SetPrompt(value);
                break;
            case "repeatable":
                // Тут можна обробити логіку для повторюваних тегів
                break;
            case "name":
                DialogueUI.Instance.SetSpeakerName(value);
                break;
            case "avatar":
                DialogueUI.Instance.SetSpeakerAvatar(value);
                break;
        }
    }
    private void ProcessText(string text)
    {
        Debug.Log(text);
        PassTextToUI(text);
        // Тут можна додати додаткову обробку тексту, якщо потрібно
    }

    private void PassTextToUI(string text)
    {
        DialogueUI.Instance.ShowText(text);
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
