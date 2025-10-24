using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;
using DG.Tweening;

public class HandDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public SplineContainer splineContainer;

    public List<GameObject> hand;

    public void updateDisplay() {
        Spline spline = splineContainer.Spline;
        float spacing = 0.1f;
        float firstPosition = 0.5f - ((hand.Count-1f) * (spacing/2f));

        for(int i = 0; i < hand.Count; i++){
            float p = firstPosition + (i*spacing);
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up,forward).normalized);

            hand[i].transform.DOLocalMove(splinePosition, 0.25f);
            hand[i].transform.DORotate(rotation.eulerAngles, 0.25f);
        }
    }

    public void addCard(Card card) {
        GameObject cardDisplay = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform);
        cardDisplay.GetComponent<CardDisplay>().card = card;
        cardDisplay.transform.localScale = Vector3.zero;
        cardDisplay.transform.DOScale(Vector3.one, 0.25f);
        hand.Add(cardDisplay);
        updateDisplay();
    }
}
