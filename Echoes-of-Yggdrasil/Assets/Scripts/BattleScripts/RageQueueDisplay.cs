using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class RageQueueDisplay : MonoBehaviour
{
    public static RageQueueDisplay Instance { get; private set; }

    public GameObject rageCardPrefab;
    public GameObject emptyCardPrefab;
    public List<GameObject> rageQueue;
    public List<GameObject> emptySpaces;

    public float baseScale;

    public float getCardSpacing() {
        int maxRageQueue = BattlePlayer.Instance.getMaxRageQueue();
        return 80f/maxRageQueue;
    }

    public void updateEmpties() {
        int maxRageQueue = BattlePlayer.Instance.getMaxRageQueue();
        for (int i = 0; i < 20; i++) {
            Destroy(emptySpaces[i]);
            if (i >= rageQueue.Count && i < maxRageQueue) {
                emptySpaces[i] = Instantiate(emptyCardPrefab, transform.position, Quaternion.identity, transform);

                float spacing = getCardSpacing();

                emptySpaces[i].transform.localPosition = new Vector3(i*spacing, 0f, -1f);
                emptySpaces[i].transform.localScale = new Vector3(baseScale, baseScale, baseScale);
            }
        }
    }

    public void updateCard(int i) {
        float spacing = getCardSpacing();

        rageQueue[i].transform.localPosition = new Vector3(i*spacing, 0f, 0f);
        rageQueue[i].transform.localScale = new Vector3(baseScale, baseScale, baseScale);
    }

    public void updateDisplay() {
        updateEmpties();
        for(int i = 0; i < rageQueue.Count; i++){
            updateCard(i);
        }
    }

    public void popDisplay(int i, float scale, float speedMultipler=1) {
        Vector3 newScale = rageQueue[i].transform.localScale * scale;
        rageQueue[i].transform.DOScale(newScale, 0.14f/speedMultipler);
    }

    // Resets popDisplay
    public void resetDisplay(int i, float speedMultipler=1) {
        Vector3 newScale = new Vector3(baseScale, baseScale, baseScale);
        rageQueue[i].transform.DOScale(newScale, 0.02f/speedMultipler);
    }

    public void addCard(Card card) {
        int maxRageQueue = BattlePlayer.Instance.getMaxRageQueue();
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
        baseScale = 2f;
    }

    void Start() {
        for (int i = 0; i < 20; i++) {
            GameObject emptySpace = Instantiate(emptyCardPrefab, transform.position, Quaternion.identity, transform);
            emptySpaces.Add(emptySpace);
        }
        updateDisplay();
    }
}
