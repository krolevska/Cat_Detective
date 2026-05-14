using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System;

public class DialogueController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Links")]
    [SerializeField] private GameObject dialoguePanel; // UI panel for dialogue
    [SerializeField] private TextAsset inkJSONAsset = null; // Ink JSON asset for the story
    [SerializeField] private GameObject player; // Link to the player
    [SerializeField] private Image npcImage; // UI element for NPC image
    [SerializeField] private TextMeshProUGUI npcNameTextPrefab; // Prefab for NPC name text
    [SerializeField] private TextMeshProUGUI dialogueTextPrefab; // Prefab for dialogue text
    [SerializeField] private Button choiceButtonPrefab; // Prefab for choice buttons
    #endregion

    #region Variables


    [Header("Variables")]
    public int reputationLevel = 0;
    public int money = 0;
    public int cigarettes = 0;
    public string[] inventoryItems; // Players inventory items
    public string[] questInfo; // Information about quests (e.g., active quests, completed quests, etc.)

    [Header("Settings")]
    public string npcName = "NPC Name"; // NPC name to display in the dialogue
    public Sprite npcSprite; // NPC image to display in the dialogue


    [Header("Story Variables")]
    public Story story;
    public static event Action<Story> OnCreateStory;

    #endregion
    /*
       Functions: 
       Get information from the script
       Show information in the dialogue window
       Get information from the character's data and write it to the scenario
       Get information from the scenario and write it to the character's data
   */

    /// <summary>
    /// Starts the dialogue with the NPC, initializes the story, and updates the UI accordingly
    /// </summary>
    public void StartDialogue()
    {
        // if using new Input System, switch to UI action map or disable player controls here
        // playerInput?.SwitchCurrentActionMap("UI");

        RemoveChildren();
        Debug.Log("Starting dialogue with " + gameObject.name);
        dialoguePanel.SetActive(true);
        StartStory();
        player.GetComponent<PlayerMovement>().MoveSpeed = 0;
        player.GetComponent<PlayerInteraction>().InteractAction.action.Disable(); // Disable the interact action to prevent further interactions while the dialogue is active
    }
    /// <summary>
    /// Removes all child objects from the dialogue panel to clear previous dialogue content before displaying new content
    /// </summary>
    private void RemoveChildren()
    {
        int childCount = dialoguePanel.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(dialoguePanel.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// Initializes the Ink story from the provided JSON asset, invokes the OnCreateStory event for any listeners, and refreshes the dialogue UI to display the initial content of the story
    /// </summary>
    private void StartStory()
    {
        var inkJSON = inkJSONAsset.text;
        story = new Story(inkJSON);
        if (OnCreateStory != null) OnCreateStory(story);
        RefreshView();
    }
    /// <summary>
    /// Refreshes the dialogue UI by clearing existing content, displaying the current dialogue text from the Ink story, and creating buttons for any available choices. If there are no choices left, it creates a button to end the dialogue.
    /// </summary>
    private void RefreshView()
    {
        RemoveChildren();

        while (story.canContinue)
        {
            string text = story.Continue(); // next line of dialogue
            text = text.Trim(); // remove any excess whitespace
            Debug.Log(text);
            CreateContentView(text);
        }

        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceButton(choice.text.Trim(), i); // create a button for each choice
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });
            }
        }
        // If there are no choices left, it means the story has reached an end point, so we create a button to allow the player to end the dialogue.
        else
        {
            Button choice = CreateChoiceButton("Завершити діалог", 0);
            choice.onClick.AddListener(EndDialogue); // when clicked, it will call the EndDialogue method to close the dialogue panel and clean up the UI.

        }
    }
    public void EndDialogue()
    {
        // if using new Input System, switch back to player action map or re-enable player controls here
        // playerInput?.SwitchCurrentActionMap("Player");

        dialoguePanel.SetActive(false);
        RemoveChildren();

        // Depending on how you want to manage the story state,
        // you can either reset the story to its initial state
        // or set it to null to create a new instance when starting a new dialogue.
        // This allows for flexibility in how you want to handle multiple dialogues
        // with the same NPC or different NPCs.
        // story?.ResetState(); // or
        story = null;
        dialoguePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().MoveSpeed = 5;
        player.GetComponent<PlayerInteraction>().InteractAction.action.Enable(); // Enable the interact action to prevent further interactions while the dialogue is active

    }


    /// <summary>
    /// Creates the content view for the dialogue, including the NPC image, name, and dialogue text. This method is called for each line of dialogue that can be continued in the Ink story. It instantiates UI elements for the NPC's image and name, as well as the dialogue text, and populates them with the appropriate content from the story.
    /// </summary>
    /// <param name="text"></param>
    void CreateContentView(string text)
    {
        Instantiate(npcImage, dialoguePanel.transform).sprite = npcSprite; // display the NPC's image in the dialogue panel
        TextMeshProUGUI npcNameText = Instantiate(npcNameTextPrefab, dialoguePanel.transform);
        npcNameText.text = npcName;
        TextMeshProUGUI dialogueText = Instantiate(dialogueTextPrefab, dialoguePanel.transform);
        dialogueText.text = text;
    }
    /// <summary>
    /// Creates a button for a given choice in the Ink story. This method is called for each available choice when refreshing the dialogue view. It instantiates a button from the provided prefab, sets its parent to the dialogue panel, and positions it based on the index of the choice. The button's text is set to the choice's text, and an event listener is added to handle clicks on the button, which will trigger the OnClickChoiceButton method with the corresponding choice.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    Button CreateChoiceButton(string text, int index)
    {

        Button button = Instantiate(choiceButtonPrefab) as Button;
        button.transform.SetParent(dialoguePanel.transform, false);

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        var rt = button.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(300f, -130 - index * 50f); // position the button based on the index to create a vertical list of choices

        return button;
    }
    /// <summary>
    /// Handles the click event for a choice button. When a choice button is clicked, this method is called with the corresponding Choice object from the Ink story. It updates the story's state by choosing the index of the selected choice, and then refreshes the dialogue view to display the new content based on the player's selection. This allows the dialogue to branch and progress according to the player's choices in the story.
    /// </summary>
    /// <param name="choice"></param>
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }
}
