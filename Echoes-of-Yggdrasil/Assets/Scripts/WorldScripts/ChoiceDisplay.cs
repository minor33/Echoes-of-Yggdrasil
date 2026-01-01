using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceDisplay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Choice choice;

    [SerializeField] private TMP_Text choiceText;
    [SerializeField] private GameObject choiceDescription;
    [SerializeField] private TMP_Text choiceDescriptionText;

    void Start()
    {
        choiceText.text = choice.text;
        choiceDescriptionText.text = choice.description;
        choiceDescription.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData){
        choiceDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData){
        choiceDescription.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData){
        ChoiceHandler.Instance.makeChoice(CampaignManager.Instance.getCurrentEvent().name, transform.GetSiblingIndex());
    }
}
