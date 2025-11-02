using UnityEngine;
using UnityEditor;
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

    public Image cardGlow;

    public Color basicGlowColor;
    public Color chooseGlowColor;

    private bool dragging;
    private bool hovering;
    public bool expanded;

    private int glowPulse;
    private int siblingIndex;

    private const float PLAY_HEIGHT = 1.65f;

    public void OnPointerDown(PointerEventData eventData){
        if(handDisplay.isBusy() || dragging || !expanded || !BattlePlayer.Instance.isPlayerTurn()){
            return;
        }
        Card card = BattlePlayer.Instance.getCard(siblingIndex);
        if(card.getEnergy() > BattlePlayer.Instance.getEnergy()){
            return;
        }
        canvasGroup.blocksRaycasts = false;
        dragging = true; 
    }

    public void OnPointerUp(PointerEventData eventData){
        if(!dragging){
            return;
        }
        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            Card card = BattlePlayer.Instance.getCard(siblingIndex);
            if(hasChoose){
                GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
                if(targetObject != null){
                    Enemy targetEnemy = targetObject.GetComponentInParent<Enemy>();
                    BattlePlayer.Instance.playCard(siblingIndex, targetEnemy);
                }
            } else {
                BattlePlayer.Instance.playCard(siblingIndex);
            }
        }
        dragging = false;
        hovering = false;
        canvasGroup.blocksRaycasts = true;
        handDisplay.updateDisplay();
    }

    public void OnDrag(PointerEventData eventData){
        if(!dragging){
            return;
        }

        rectTransform.anchoredPosition += eventData.delta / primaryCanvas.scaleFactor;

        if(rectTransform.anchoredPosition.y > PLAY_HEIGHT){
            if(hasChoose){
                GameObject target = eventData.pointerCurrentRaycast.gameObject;
                if(target != null){
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
        hovering = true;
    }
    public void OnPointerExit(PointerEventData eventData){
        /*
        if(siblingIndex == 0){
            Debug.Log("Pointer exit");
        }
        */
        if(!dragging){
            hovering = false;
            if(!handDisplay.isBusy()){
                if(expanded){
                    expanded = false;
                    handDisplay.updateDisplay(0);
                }
            }
        }
    }

    void Update() {
        
        if(siblingIndex == 0){
            Debug.Log($"Dragging: {dragging}, Hovering: {hovering}, Expanded: {expanded}, HandBusy: {handDisplay.isBusy()}, DrawingCards: {BattlePlayer.Instance.isDrawing()}");
        }
        
        if(hovering){
            if(!handDisplay.isBusy() && !BattlePlayer.Instance.isDrawing()){
                if(!expanded){
                    expanded = true;
                    rectTransform.localScale = rectTransform.localScale * 1.2f;
                    rectTransform.rotation = Quaternion.identity;
                    Vector3 newPosition = rectTransform.localPosition;
                    newPosition.y = 0.49f;
                    rectTransform.localPosition = newPosition;
                    siblingIndex = transform.GetSiblingIndex();
                    transform.SetAsLastSibling();
                    handDisplay.pushRight(siblingIndex);
                }                
            } else {
                if(expanded){
                    expanded = false;
                }
            }
        }

        if(cardGlow.enabled){
            glowPulse = (glowPulse+1)%350;
            Color tempColor = cardGlow.color;
            tempColor.a = (float)(glowPulse+500) / 1000;
            cardGlow.color = tempColor;
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
        glowPulse = 0;
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