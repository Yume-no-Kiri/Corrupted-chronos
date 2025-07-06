using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class DialogueChioceButton : MonoBehaviour, ISelectHandler
{
    [Header("Components:")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choice_text;

    private int choice_index = -1;
    public void SetChoiceText(string choice_txt)
    {
        choice_text.text = choice_txt;
    }

    public void SetChoiceIndex(int choice_index)
    {
        this.choice_index = choice_index;
    }

    public void SelectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventsManager.instance.dialogue_events.UpdateChoice(choice_index);
    }
}
