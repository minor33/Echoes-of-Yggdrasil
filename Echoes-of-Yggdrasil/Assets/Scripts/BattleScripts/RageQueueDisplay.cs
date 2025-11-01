using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class RageQueueDisplay : MonoBehaviour
{
    public static RageQueueDisplay Instance { get; private set; }

    public GameObject rageCardPrefab;
    public List<GameObject> rageQueue;

    public void updateCard(int i) {
        float spacing = 6f;
        float firstPosition = 0f;

        rageQueue[i].transform.localPosition = new Vector3(i*spacing + firstPosition, 0f, 0f);
        rageQueue[i].transform.localScale = new Vector3(2f, 2f, 2f);
    }

    public void updateDisplay() {
        for(int i = 0; i < rageQueue.Count; i++){
            updateCard(i);
        }
    }

    public void popDisplay(int i) {
        Vector3 newScale = rageQueue[i].transform.localScale * 1.1f;
        rageQueue[i].transform.DOScale(newScale, 0.14f);
    }

    public void addCard(Card card) {
        GameObject cardDisplay = Instantiate(rageCardPrefab, transform.position, Quaternion.identity, transform);
        cardDisplay.GetComponent<CardDisplay>().card = card;
        // cardDisplay.transform.localScale = Vector3.zero;
        rageQueue.Add(cardDisplay);
        updateDisplay();
    }

    public void removeCard(int index){
        Destroy(rageQueue[index]);
        rageQueue.RemoveAt(index);
    }

    public void clear() {
        int size = rageQueue.Count;
        for(int i = 0; i < size; i++){
            removeCard(0);
        }
        updateDisplay();
    }

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
