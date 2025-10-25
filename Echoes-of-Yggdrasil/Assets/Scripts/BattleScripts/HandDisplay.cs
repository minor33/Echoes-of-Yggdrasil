using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;
using DG.Tweening;

public class HandDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public SplineContainer splineContainer;

    public List<GameObject> hand;

    public void updateCard(int i, float time) {
        Spline spline = splineContainer.Spline;
        float spacing = 0.1f;
        float firstPosition = 0.5f - ((hand.Count-1f) * (spacing/2f));

        float p = firstPosition + (i*spacing);
        Vector3 splinePosition = spline.EvaluatePosition(p);
        Vector3 forward = spline.EvaluateTangent(p);
        Vector3 up = spline.EvaluateUpVector(p);
        Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up,forward).normalized);

        hand[i].transform.SetSiblingIndex(i);
        if(time > 0f){
            hand[i].transform.DOLocalMove(splinePosition, time);
            hand[i].transform.DORotate(rotation.eulerAngles, time);
            hand[i].transform.DOScale(cardPrefab.transform.localScale, time);        
        } else {
            hand[i].transform.localPosition = splinePosition;
            hand[i].transform.rotation = rotation;
            hand[i].transform.localScale = cardPrefab.transform.localScale;
        }

    }

    public void updateDisplay(float time=0) {
        for(int i = 0; i < hand.Count; i++){
            updateCard(i, time);
        }
    }

    public void pushRight(int index){
        for(int i = index+1; i < hand.Count; i++){
            RectTransform rectTransform = hand[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition += new Vector2(0.55f,0);
        }
    }

    public void addCard(Card card) {
        GameObject cardDisplay = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform);
        cardDisplay.GetComponent<CardDisplay>().card = card;
        cardDisplay.transform.localScale = Vector3.zero;
        hand.Add(cardDisplay);
        updateDisplay();
    }
}
