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
//    [SerializeField] private Transform historyRoot;
//    [SerializeField] private TMP_Text linePrefab;
    [SerializeField] private Transform choicesRoot;
//    [SerializeField] private ScrollRect scrollRect;

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
        npcNameTextPrefab.GetComponent<TextMeshProUGUI>().text = value;
    }

    public void SetSpeakerAvatar(string value)
    {
        Debug.Log($"Set speaker avatar: {value}");
        npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(value);
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
    public void ShowNPCInfo()
    {
        GameObject nameText = Instantiate(npcNameTextPrefab, dialoguePanel.transform);
        GameObject image = Instantiate(npcImage, dialoguePanel.transform);        
    }
    public void ShowText(string text)
    {
        Debug.Log($"Show text: {text}");
        GameObject dialogueText = Instantiate(dialogueTextPrefab, dialoguePanel.transform);
        TextMeshProUGUI dialogueTextComponent = dialogueText.GetComponent<TextMeshProUGUI>();
        dialogueTextComponent.text = text;
    }

    public void ShowChoices(string[] choices)
    {
        if (choices.Length > 0)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                string choice = choices[i];
                Button button = CreateChoiceButton(choice, i);
                button.onClick.AddListener(delegate
                {
                    NarrativeDirector.Instance.MakeChoice(i);
                });
                Debug.Log($"Show choice: {choice}, index {i}");
            }
        }
        else
        {
            Button choice = CreateChoiceButton("End Dialogue", 0);
            choice.onClick.AddListener(CloseDialogue);
        }
    }

    Button CreateChoiceButton(string text, int index)
    {

        Button button = Instantiate(choiceButtonPrefab).GetComponent<Button>();
        button.transform.SetParent(choicesRoot.transform, false);

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        var rt = button.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 0 - index * 50f);

        return button;
    }

    public void OpenDialogue()
    {
        isDialogueOpen = true;
        dialoguePanel.SetActive(true);
        ShowNPCInfo();
        DialogueOpened?.Invoke();
    }

    public void CloseDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);
        ClearChoices();
        RemoveChildren();
        DialogueClosed?.Invoke();
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesRoot)
            Destroy(child.gameObject);
    }
}