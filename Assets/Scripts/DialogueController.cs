using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEditor;
using System;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Links")]
    [SerializeField] private GameObject dialoguePanel; // UI елемент для діалогу
    [SerializeField] private TextAsset inkJSONAsset = null; // Файл з діалогом у форматі Ink
    [SerializeField] private GameObject player; // Посилання на гравця
    [SerializeField] private Image npcImage; // Зображення NPC
    [SerializeField] private TextMeshProUGUI npcNameTextPrefab; // Префаб для імені NPC
    [SerializeField] private TextMeshProUGUI dialogueTextPrefab; // Префаб для тексту діалогу
    [SerializeField] private Button choiceButtonPrefab; // Префаб для кнопок вибору
    #endregion

    #region Variables


    [Header("Variables")]
    public int reputationLevel = 0;
    public int money = 0;
    public int cigarettes = 0;
    public string[] inventoryItems; // Предмети в інвентарі
    public string[] questInfo; // Інформація про квести

    [Header("Settings")]
    public string npcName = "NPC Name"; // Ім'я NPC
    public Sprite npcSprite; // Зображення NPC


    [Header("Story Variables")]
    public Story story;
    public static event Action<Story> OnCreateStory;

    #endregion
    /*
     Функції
Брати інформацію з сценарію
Виводити інформацію на діалогове вікно
Брати дані з скрипта персонажа і записувати їх в сценарій
Брати дані зі сценарія і записувати їх в скрипт персонажа

*/
 
    /// <summary>
    /// Починає діалог з NPC
    /// </summary>
    public void StartDialogue()
    {
        // Видаляє дефолтне повідомлення
        RemoveChildren();
        StartStory();
        Debug.Log("Starting dialogue with " + gameObject.name);
        dialoguePanel.SetActive(true);
        StartStory();
    }
    /// <summary>
    /// Видаляє всі дочірні елементи з панелі діалогу
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
    /// Ініціалізує історію з Ink JSON та починає діалог
    /// </summary>
    private void StartStory()
    {
        var inkJSON = inkJSONAsset.text;
        story = new Story(inkJSON);
        if (OnCreateStory != null) OnCreateStory(story);
        RefreshView();
    }
    /// <summary>
    /// Оновлює UI відповідно до поточного стану історії
    /// </summary>
    private void RefreshView()
    {
        RemoveChildren();

        while (story.canContinue)
        {
            string text = story.Continue(); // отримуємо наступний рядок діалогу
            text = text.Trim(); // видаляємо зайві пробіли
            Debug.Log(text);
            CreateContentView(text);
        }

        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceButton(choice.text.Trim(), i); // створюємо кнопку для кожного варіанту вибору
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });
            }
        }
        // Якщо немає варіантів вибору, діалог завершується
        else
        {
            Button choice = CreateChoiceButton("Завершити діалог", 0);
            choice.onClick.AddListener(EndDialogue); // метод-група, без лямбди

        }
    }
    public void EndDialogue()
    {
        // якщо використовуєш новий Input System — поверни геймплейну мапу або ввімкни Interact
        // playerInput?.SwitchCurrentActionMap("Player"); // якщо перемикався на "UI"
        // interactAction?.Enable();                     // якщо просто вимикав дію

        dialoguePanel.SetActive(false);
        RemoveChildren();

        // опційно: скинути історію, щоб наступного разу почати з початку
        // story?.ResetState(); // або
        // story = null;        // і створити заново в StartStory()
    }


    /// <summary>
    /// Створює елементи UI для відображення діалогу NPC
    /// </summary>
    /// <param name="text"></param>
    void CreateContentView(string text)
    {
        Instantiate(npcImage, dialoguePanel.transform).sprite = npcSprite; // Встановлюємо зображення 
        TextMeshProUGUI npcNameText = Instantiate(npcNameTextPrefab, dialoguePanel.transform);
        npcNameText.text = npcName;
        TextMeshProUGUI dialogueText = Instantiate(dialogueTextPrefab, dialoguePanel.transform);
        dialogueText.text = text;
    }
    /// <summary>
    /// Створює кнопку для варіантів вибору
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
        rt.anchoredPosition = new Vector2(300f, -130 - index * 50f); // кожна нижче на 50

        return button;
    }
    /// <summary>
    /// Обробка натискання кнопки вибору
    /// </summary>
    /// <param name="choice"></param>
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }
}
