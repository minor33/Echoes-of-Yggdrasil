using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeityDisplay : MonoBehaviour, IPointerDownHandler
{
    public Deity deity;

    [SerializeField] private TMP_Text deityName;
    [SerializeField] private Image deityBackgroundImage;
    [SerializeField] private Image deityGlow;

    private bool selected;

    public void OnPointerDown(PointerEventData eventData){
        if(selected){
            MainMenuManager.Instance.unselectDeity(deity);
            selected = false;
        } else {
            selected = MainMenuManager.Instance.selectDeity(deity);
        }
    }

    void Start()
    {
        deityName.text = deity.name;
        deityBackgroundImage.color = deity.color;
        deityGlow.enabled = false;
        selected = false;
    }

    void Update(){
        deityGlow.enabled = selected;
    }
}
