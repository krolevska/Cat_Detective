using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private GameObject dialogueTextPrefab;
    [SerializeField] private Button choiceButtonPrefab;
    //    [SerializeField] private Transform historyRoot;
    //    [SerializeField] private TMP_Text linePrefab;
    [SerializeField] private Transform choicesRoot;
    [SerializeField] private Transform textRoot;
    [SerializeField] private Transform npcImageRoot;
    [SerializeField] private Transform npcNameRoot;
    //    [SerializeField] private ScrollRect scrollRect;

    private bool isDialogueOpen = false;
    public bool IsDialogueOpen => isDialogueOpen;

    public event Action DialogueOpened;
    public event Action DialogueClosed;
    public event Action<int> ChoiceSelected;

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
        speakerNameText.text = value;
    }

    public void SetSpeakerAvatar(string value)
    {
        Debug.Log($"Set speaker avatar: {value}");
        speakerImage.sprite = Resources.Load<Sprite>(value);
    }

    public void SetPrompt(string value)
    {
        Debug.Log($"Set prompt: {value}");

    }

    public void ClearText()
    {
        foreach (Transform child in textRoot)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesRoot)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearDialogue()
    {
        ClearText();
        ClearChoices();
    }
    public void ShowText(string text)
    {
        Debug.Log($"Show text: {text}");

        GameObject dialogueText = Instantiate(dialogueTextPrefab, textRoot);
        TextMeshProUGUI dialogueTextComponent = dialogueText.GetComponent<TextMeshProUGUI>();
        dialogueTextComponent.text = text;
    }

    public void ShowChoices(DialogueChoiceData[] choices)
    {
        ClearChoices();

        if (choices.Length > 0)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                int buttonIndex = i;
                int inkChoiceIndex = choices[i].InkChoiceIndex;
                string choiceText = choices[i].Text;

                Button button = CreateChoiceButton(choiceText, buttonIndex);

                button.onClick.AddListener(() =>
                {
                    ChoiceSelected?.Invoke(inkChoiceIndex);
                });
            }
        }
        else
        {
            Button button = CreateChoiceButton("Вийти", 0);
            button.onClick.AddListener(CloseDialogue);
        }
    }

    Button CreateChoiceButton(string text, int index)
    {

        Button button = Instantiate(choiceButtonPrefab).GetComponent<Button>();
        button.transform.SetParent(choicesRoot.transform, false);

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        return button;
    }

    public void OpenDialogue()
    {
        isDialogueOpen = true;
        dialoguePanel.SetActive(true);

        ClearDialogue();

        DialogueOpened?.Invoke();
    }

    public void CloseDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);

        ClearDialogue();

        DialogueClosed?.Invoke();
    }
}