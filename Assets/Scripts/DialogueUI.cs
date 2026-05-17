using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject npcImage;
    [SerializeField] private GameObject npcNameTextPrefab;
    [SerializeField] private GameObject dialogueTextPrefab;
    [SerializeField] private GameObject choiceButtonPrefab;

    public void RemoveChildren()
    {
        foreach (Transform child in dialoguePanel.transform)
        {
            Destroy(child.gameObject);
        }
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
}