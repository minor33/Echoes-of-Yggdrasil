using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : Unit {
    public static BattlePlayer Instance { get; private set; }

    public TMP_Text healthText;
    public Image healthBarFill;

    public int rage;
    public int maxRage;

    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;
    public List<Card> rageQueue;

    public Card getCard(int index) {
        return hand[index];
    }

    public void shuffleDeck()
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    public void reshuffleDiscard(){
        deck.AddRange(discard);
        discard.Clear();
    }

    public void drawCard() {
        if(hand.Count == 10){
            return;
        }
        if(deck.Count == 0){
            reshuffleDiscard();
            if(deck.Count == 0){
                Debug.Log("Hand empty");
                return;
            }
        }
        Card drawnCard = deck[0];
        hand.Add(drawnCard);
        HandDisplay.Instance.addCard(drawnCard);
        deck.RemoveAt(0);
    }

    public void playCard(int index, Enemy targetEnemy = null) {
        Card playedCard = hand[index];
        playedCard.play(targetEnemy);
        addCardToRageQueue(index);
        removeCard(index);
        BattlePlayer.Instance.checkRage();
    }

    public void addCardToRageQueue(int index) {
        // TODO: Push and shit and max stuff
        Card playedCard = hand[index];
        rageQueue.Add(playedCard);
        RageQueueDisplay.Instance.addCard(playedCard);
    }

    public void removeCard(int index){
        hand.RemoveAt(index);
        HandDisplay.Instance.removeCard(index);
    }

    public void debugPrintHand() {
        string message = "Hand: ";
        for(int i = 0; i < hand.Count; i++){
            message += $"{hand[i].getName()}, ";
        }
        Debug.Log(message);
    }

    public override void die() {
        Debug.Log("You died");
    }

    public void updateDisplay() {
        healthText.text = $"{health}/{maxHealth}";
        healthBarFill.fillAmount = (float)health / (float)maxHealth;
    }

    public void checkRage() {
        if (rage >= maxRage) {
            foreach (Card card in rageQueue) {
                card.triggerRage();
            }
            rage = 0;
            rageQueue.Clear();
            RageQueueDisplay.Instance.clear();
        }

    }

    void Update() {
        updateDisplay();
    }

    void Start() {
        hand = new List<Card>();
        deck = new List<Card>();
        discard = new List<Card>();
        rageQueue = new List<Card>();

        maxHealth = 100;
        health = maxHealth;

        maxRage = 10;
        rage = 0;
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