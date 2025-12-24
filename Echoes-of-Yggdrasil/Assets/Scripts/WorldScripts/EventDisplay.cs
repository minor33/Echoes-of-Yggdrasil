using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventDisplay : MonoBehaviour, IPointerDownHandler
{
    public Event ev;

    [SerializeField] private TMP_Text eventName;
    [SerializeField] private Image eventBackgroundImage;

    void Start()
    {
        eventName.text = ev.name;
        eventBackgroundImage.color = ev.color;
    }

    public void OnPointerDown(PointerEventData eventData){
        
    }
}
