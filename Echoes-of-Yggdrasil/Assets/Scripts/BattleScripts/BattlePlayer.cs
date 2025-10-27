using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : Unit {
    public static BattlePlayer Instance { get; private set; }

    public TMP_Text deckText;
    public TMP_Text discardText;
    public TMP_Text energyText;
    public TMP_Text rageText;

    public int rage;
    public int maxRage;
    public int energy;
    public int maxEnergy;

    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;
    public List<Card> rageQueue;

    public Card getCard(int index) {
        return hand[index];
    }

    public int getEnergy(){
        return energy;
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

    public void discardHand(){
        for(int i = hand.Count-1; i >= 0; i--){
            discard.Add(hand[i]);
            removeCard(i);
        }
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
        energy -= playedCard.getEnergy();
        removeCard(index);
        playedCard.play(targetEnemy);
        addCardToRageQueue(playedCard);
        discard.Add(playedCard);
        BattlePlayer.Instance.checkRage();
    }

    public void addCardToRageQueue(Card playedCard) {
        // TODO: Push and shit and max stuff
        rageQueue.Add(playedCard);
        RageQueueDisplay.Instance.addCard(playedCard);
    }

    public void removeCard(int index){
        hand.RemoveAt(index);
        HandDisplay.Instance.removeCard(index);
    }

    public override void die() {
        Debug.Log("You died");
    }

    public void updateDisplay() {
        updateHealthbar();
        deckText.text = $"{deck.Count}";
        discardText.text = $"{discard.Count}";
        energyText.text = $"{energy} / {maxEnergy}";
        rageText.text = $"{rage} / {maxRage}";
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

    public void startTurn(){
        energy = maxEnergy;
        block = 0;
        for(int i = 0; i < 5; i++){
            drawCard();
        }
    }

    public void endTurn(){
        discardHand();
        BattleManager.Instance.progressBattleState();
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

        maxEnergy = 3;
        energy = 3;
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