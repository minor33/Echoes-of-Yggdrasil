using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceDisplay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Choice choice;

    [SerializeField] private TMP_Text choiceText;

    void Start()
    {
        choiceText.text = choice.text;
    }

    public void OnPointerEnter(PointerEventData eventData){

    }

    public void OnPointerExit(PointerEventData eventData){

    }

    public void OnPointerDown(PointerEventData eventData){

    }
}
