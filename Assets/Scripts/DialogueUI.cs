using Ink.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject npcImage;
    [SerializeField] private GameObject npcNameTextPrefab;
    [SerializeField] private GameObject dialogueTextPrefab;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Transform historyRoot;
    [SerializeField] private TMP_Text linePrefab;
    [SerializeField] private Transform choicesRoot;
    [SerializeField] private ScrollRect scrollRect;

    private Story story;
    private bool isDialogueOpen = false;
    public bool IsDialogueOpen => isDialogueOpen;

    public event Action DialogueOpened;
    public event Action DialogueClosed;

    public static DialogueUI Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dialoguePanel.SetActive(false);

    }

    public void SetTextStyle(string value)
    {
        Debug.Log($"Set text style: {value}");

        switch (value)
        {
            case "narration":
                break;
            case "protagonist":
                break;
            case "npc":
                break;
            case "inner_thought":
                break;
            case "document_text":
                break;
            case "system_fact":
                break;
            case "system_quest":
                break;
            case "system_quest_complete":
                break;
            case "system_conclusion":
                break;
            default:
                break;
        }
    }
    public void SetSpeakerName(string value)
    {
        Debug.Log($"Set speaker name: {value}");

    }

    public void SetSpeakerAvatar(string value)
    {
        Debug.Log($"Set speaker avatar: {value}");
    }

    public void SetPrompt(string value)
    {
        Debug.Log($"Set prompt: {value}");
    }


    public void RemoveChildren()
    {
        foreach (Transform child in dialoguePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshView(Story story)
    {
        RemoveChildren();
        // ShowNPCInfo();
        ShowText("This is a sample dialogue text. Replace this with actual dialogue from the Ink story.");
        ShowChoices(story);
    }

    public void ShowNPCInfo(string npcName, Sprite npcSprite)
    {
        GameObject nameText = Instantiate(npcNameTextPrefab, dialoguePanel.transform);
        TextMeshProUGUI nameTextComponent = nameText.GetComponent<TextMeshProUGUI>();
        nameTextComponent.text = npcName;
        GameObject image = Instantiate(npcImage, dialoguePanel.transform);
        Image imageComponent = image.GetComponent<Image>();
        imageComponent.sprite = npcSprite;
    }
    public void ShowText(string text)
    {
        Debug.Log($"Show text: {text}");
        GameObject dialogueText = Instantiate(dialogueTextPrefab, dialoguePanel.transform);
        TextMeshProUGUI dialogueTextComponent = dialogueText.GetComponent<TextMeshProUGUI>();
        dialogueTextComponent.text = text;
    }

    public void ShowChoices(Story story)
    {
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceButton(choice.text.Trim(), i);
                button.onClick.AddListener(delegate
                {
                    story.ChooseChoiceIndex(choice.index);
                });
            }
        }
        else
        {
            Button choice = CreateChoiceButton("End Dialogue", 0);
            choice.onClick.AddListener(RemoveChildren);
        }
    }

    Button CreateChoiceButton(string text, int index)
    {

        Button button = Instantiate(choiceButtonPrefab).GetComponent<Button>();
        button.transform.SetParent(dialoguePanel.transform, false);

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        var rt = button.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(300f, -130 - index * 50f);

        return button;
    }

    private void OpenDialogue()
    {
        isDialogueOpen = true;
        dialoguePanel.SetActive(true);
        DialogueOpened?.Invoke();
    }

    private void CloseDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);
        ClearChoices();
        DialogueClosed?.Invoke();
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesRoot)
            Destroy(child.gameObject);
    }
}