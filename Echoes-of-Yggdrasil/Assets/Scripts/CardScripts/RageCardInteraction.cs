using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using static GameConstants;

public class RageCardInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;

    private bool hovering;
    private bool selected;
    private float baseScale;
    public Image cardGlow; 

    public void OnPointerUp(PointerEventData eventData) {

    }

    public void OnPointerDown(PointerEventData eventData) {
        if (RageQueueDisplay.Instance.selecting) {
            if (selected) {
                deselect();
            } else {
                select();
            }
        }
    }
    
    public void select() {
        selected = true;
        cardGlow.enabled = selected;
        RageQueueDisplay.Instance.selectedCards.Add(this);
    }

    public void deselect() {
        selected = false;
        cardGlow.enabled = selected;
        RageQueueDisplay.Instance.selectedCards.Remove(this);
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (!hovering) {
            hovering = true;
            rectTransform.localScale = new Vector3(baseScale*1.1f, baseScale*1.1f, baseScale*1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if (hovering) {
            hovering = false;
            rectTransform.localScale = new Vector3(baseScale, baseScale, baseScale);
        }
        
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseScale = RageQueueDisplay.Instance.baseScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        selected = false;
        cardGlow.enabled = false;
    }
}
