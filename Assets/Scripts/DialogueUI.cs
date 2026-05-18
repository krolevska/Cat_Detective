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

    public void Advance()
    {
        if (!isDialogueOpen)
            return;

        if (choicesRoot.childCount > 0)
            return;

        if (story.canContinue)
        {
            ShowNextBlock();
            return;
        }

        if (story.currentChoices.Count == 0)
        {
            CloseDialogue();
        }
    }

    private void ShowNextBlock()
    {
        ClearChoices();

        while (story.canContinue)
        {
            string line = story.Continue().Trim();
            List<string> tags = story.currentTags;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            AddLineToHistory(line, tags);

            if (!story.canContinue && story.currentChoices.Count > 0)
                ShowChoices();

            return;
        }

        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
    }

    private void ShowChoices()
    {
        ClearChoices();

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            int choiceIndex = i;
            Choice choice = story.currentChoices[i];

            Button button = Instantiate(choiceButtonPrefab, choicesRoot);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();


            button.onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(choiceIndex);
                ClearChoices();
                ShowNextBlock();
            });
        }
    }
    private void AddLineToHistory(string rawLine, List<string> tags)
    {
        TMP_Text line = Instantiate(linePrefab, historyRoot);
        line.text = FormatLineForDialogueFeed(rawLine, tags);

        Canvas.ForceUpdateCanvases();

        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }
    private string FormatLineForDialogueFeed(string line, List<string> tags)
    {
        // Системні повідомлення: [НОВИЙ ФАКТ...], [ЗАВДАННЯ...]
        if (line.StartsWith("[") && line.EndsWith("]"))
        {
            return $"<i>{line}</i>";
        }

        int colonIndex = line.IndexOf(':');

        if (
            colonIndex > 0 &&
            colonIndex < 35 &&
            !line.StartsWith("<b>") &&
            !line.StartsWith("[")
        )
        {
            string speaker = line.Substring(0, colonIndex).Trim();
            string body = line.Substring(colonIndex + 1).Trim();

            return $"<b>{speaker}</b>\n{body}";
        }

        return line;
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