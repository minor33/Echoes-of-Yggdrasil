using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private HandDisplay handDisplay;
    private Canvas primaryCanvas;
    private CanvasGroup canvasGroup;
    private BattleManager battleManager;

    public Image cardGlow;

    private bool dragging;
    private int siblingIndex;

    private const float PLAY_HEIGHT = 1.75f;

    public void OnPointerDown(PointerEventData eventData){
        canvasGroup.blocksRaycasts = false;
        dragging = true; 
    }
    public void OnPointerUp(PointerEventData eventData){
        dragging = false;
        canvasGroup.blocksRaycasts = true;

        // Play Card:
        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            BattlePlayer battlePlayer = BattlePlayer.Instance;
            Ability ability = battlePlayer.getCard(siblingIndex).getPlayAbility();
            if(ability.hasChoose()){
                // Some nonsense
            } else {
                ability.triggerAbility();
                battlePlayer.removeCard(siblingIndex);
            }
        }

        handDisplay.updateDisplay();
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta / primaryCanvas.scaleFactor;
        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            cardGlow.enabled = true;
        } else {
            cardGlow.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(!dragging){
            rectTransform.localScale = rectTransform.localScale * 1.4f;
            rectTransform.rotation = Quaternion.identity;
            Vector3 newPosition = rectTransform.localPosition;
            newPosition.y = 0.666f;
            rectTransform.localPosition = newPosition;
            siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            handDisplay.pushRight(siblingIndex);
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        if(!dragging){
            handDisplay.updateDisplay(0);
        }
    }


    void Awake() {
        handDisplay = HandDisplay.Instance;
        battleManager = BattleManager.Instance;
        rectTransform = GetComponent<RectTransform>();
        primaryCanvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        cardGlow.enabled = false;
    }
}