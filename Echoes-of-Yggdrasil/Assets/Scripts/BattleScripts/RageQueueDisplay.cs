using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class RageQueueDisplay : MonoBehaviour
{
    public static RageQueueDisplay Instance { get; private set; }

    public GameObject emptyCardPrefab;
    private List<GameObject> rageQueue; // Direct copy of rageQueue from BattlePlayer, this is not real
    private List<GameObject> emptySpaces;

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

        GameObject card = rageQueue[i];
        card.GetComponent<CardDisplay>().updateDisplay();
        card.transform.localPosition = new Vector3(i*spacing, 0f, 0f);
        card.transform.localScale = new Vector3(baseScale, baseScale, baseScale);
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

    // These can be removed later once we're sure they're not being accidentally used
    public void addCard(Card card) {
        Debug.LogError("RageQueueDisplay addCard is no longer in use");
    }

    public void createCard() {
        Debug.LogError("RageQueueDisplay createCard is no longer in use");
    }
    
    public void removeCard(int index){
        Debug.LogError("RageQueueDisplay removeCard is no longer in use");
    }

    public void swap(int index1, int index2) {
        Debug.LogError("RageQueueDisplay swap is no longer in use");
    }

    public void deselectAll() {
        Debug.LogError("RageQueueDisplay deselectAll is no longer in use");
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
        rageQueue = BattlePlayer.Instance.rageQueue;
        emptySpaces = new List<GameObject>();
        for (int i = 0; i < 20; i++) {
            GameObject emptySpace = Instantiate(emptyCardPrefab, transform.position, Quaternion.identity, transform);
            emptySpaces.Add(emptySpace);
        }
        updateDisplay();
    }
}
