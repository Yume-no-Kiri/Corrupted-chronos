using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components:")]

    [SerializeField] private GameObject content_parent;
    [SerializeField] private bool showing=false;
    [SerializeField] private TextMeshProUGUI dialogue_text;
    [SerializeField] private Image dialogue_img;
    [SerializeField] private DialogueChioceButton[] choice_buttons;
    [SerializeField] private List<Sprite> dialogueSprites;
    private void Awake()
    {
        content_parent.SetActive(showing);
        ResetPanel();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogue_events.onDialogueStarted += DialogueStarted;
        GameEventsManager.instance.dialogue_events.onDialogueFinished += DialogueFinished;
        GameEventsManager.instance.dialogue_events.onDisplayDialogue += DisplayDialogue;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogue_events.onDialogueStarted -= DialogueStarted;
        GameEventsManager.instance.dialogue_events.onDialogueFinished -= DialogueFinished;
        GameEventsManager.instance.dialogue_events.onDisplayDialogue -= DisplayDialogue;

    }

    private void DialogueStarted()
    {
        content_parent.SetActive(true);
    }
    private void DialogueFinished() 
    {
        content_parent.SetActive(false); 
        ResetPanel();
    }    

    private void DisplayDialogue(string text, List<Choice> dialogue_choices, string image)
    {
        Debug.Log(text);
        dialogue_text.text = text;
        Sprite foundSprite = dialogueSprites.Find(s => s.name == image);
        if (foundSprite != null)
        {
            dialogue_img.sprite = foundSprite;
        }
        if (dialogue_choices.Count>choice_buttons.Length)
        {
            Debug.LogError("Falten opcions de tria");
        }

        foreach (DialogueChioceButton dialogue_chioce_button in choice_buttons)
        {
            dialogue_chioce_button.gameObject.SetActive(false);
        }
            
        int choice_button_index= dialogue_choices.Count-1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogue_choices.Count; inkChoiceIndex++)
        {
            Choice dialogue_choice = dialogue_choices[inkChoiceIndex];
            DialogueChioceButton chioceButton = choice_buttons[choice_button_index];

            chioceButton.gameObject.SetActive(true);
            chioceButton.SetChoiceText(dialogue_choice.text);
            chioceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                chioceButton.SelectButton();
                GameEventsManager.instance.dialogue_events.UpdateChoice(0);
            }

            choice_button_index--;
        }
     }

    private void ResetPanel()
    {
        
        dialogue_text.text = "";  
    }
}

