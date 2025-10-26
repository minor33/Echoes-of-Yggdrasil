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
    private bool hasChoose;

    public BoxCollider2D cardCollider;
    public LayerMask targetableLayer;
    public Image cardGlow;

    public Color basicGlowColor;
    public Color chooseGlowColor;

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

        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            Ability ability = BattlePlayer.Instance.getCard(siblingIndex).getPlayAbility();
            if(hasChoose){
                Vector2 overlapSize = cardCollider.bounds.size; 
                Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, overlapSize, 0f, targetableLayer);
                if(overlaps.Length > 0){
                    Enemy target = overlaps[0].gameObject.GetComponent<Enemy>();
                    ability.triggerAbility(target);
                    BattlePlayer.Instance.removeCard(siblingIndex);
                }
            } else {
                ability.triggerAbility();
                BattlePlayer.Instance.removeCard(siblingIndex);
            }
        }
        handDisplay.updateDisplay();
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta / primaryCanvas.scaleFactor;
        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            
            if(hasChoose){
                Vector2 overlapSize = cardCollider.bounds.size; 
                Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, overlapSize, 0f, targetableLayer);
                if(overlaps.Length > 0){
                    cardGlow.enabled = true;
                } else {
                    cardGlow.enabled = false;
                }
            } else {
                cardGlow.enabled = true;
            }
        } else {
            cardGlow.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(!dragging){
            //rectTransform.localScale = rectTransform.localScale * 1.4f;
            rectTransform.localScale = rectTransform.localScale * 1.2f;
            rectTransform.rotation = Quaternion.identity;
            Vector3 newPosition = rectTransform.localPosition;
            //newPosition.y = 0.666f;
            newPosition.y = 0.49f;
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
    }

    void Start() {
        cardGlow.enabled = false;
        siblingIndex = transform.GetSiblingIndex();
        Ability ability = BattlePlayer.Instance.getCard(siblingIndex).getPlayAbility();
        if(ability.hasChoose()){
            hasChoose = true;
            cardGlow.color = chooseGlowColor;
        } else {
            hasChoose = false;
            cardGlow.color = basicGlowColor;
        }
    }
}