using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using EditorAttributes;
using static Keyword;
using static GameConstants;

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
    public int maxRageQueue;

    public int invoke;
    public int duplicate;
    public int angrier;

    private bool drawingCards;
    private bool playerTurn;

    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;
    public List<Card> rageQueue;

    [Button]
    public void GoCrazyGoWild(){
        rage = maxRage;
        checkRage();
    }

    [Button]
    public void DrinkARedBull(){
        energy = 50;
    }

    [Button]
    public void DRAAAAAAW(){
        drawCards(10);
    }

    [Button]
    public void UpdateDisplays() {
        RageQueueDisplay.Instance.updateDisplay();
    }

    public bool isPlayerTurn(){
        return playerTurn;
    }

    public bool isDrawing(){
        return drawingCards;
    }

    public Card getCard(int index) {
        return hand[index];
    }

    public int getEnergy(){
        return energy;
    }

    public int getAngrier() {
        return angrier;
    }

    public int getMaxRageQueue() {
        return maxRageQueue;
    }

    // Additional rage card triggers
    public void addInvoke(int i) {
        invoke += i;
    }

    // Additional cards added to rage queue
    public void addDuplicate(int d) {
        duplicate += d;
    }

    // Additional damage on rage queue cards
    // This should probably change to a general damage modifier so it can go negative too
    public void addAngrier(int a) {
        angrier += a;
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

    public async void drawCards(int numCards) {
        drawingCards = true;
        for(int i = 0; i < numCards; i++){
            if(hand.Count == 10){
                drawingCards = false;
                return;
            }
            if(!playerTurn){
                drawingCards = false;
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
            if(i < numCards-1){
                await Awaitable.WaitForSecondsAsync(0.202f);
            }
        }
        drawingCards = false;
    }

    public void playCard(int index, Enemy targetEnemy = null) {
        Card playedCard = hand[index];
        energy -= playedCard.getEnergy();
        removeCard(index);
        playedCard.play(targetEnemy);
        
        for (int i = 0; i < 1+duplicate; i++) {
            addCardToRageQueue(playedCard);
        } 
        duplicate = 0;
        
        discard.Add(playedCard);
        BattlePlayer.Instance.checkRage();
    }

    public void addCardToRageQueue(Card playedCard) {
        bool space = true;
        if (rageQueue.Count >= maxRageQueue) {
            space = removeFrontRageQueue();
        }
        if (space) {
            rageQueue.Add(playedCard);
            RageQueueDisplay.Instance.addCard(playedCard);
        }
    }

    public void removeCard(int index){
        hand.RemoveAt(index);
        HandDisplay.Instance.removeCard(index);
    }

    public bool removeFrontRageQueue() {
        for (int i = 0; i < rageQueue.Count; i++) {
            if (!rageQueue[i].getRageAbility().hasKeyword(STABLE)) {
                removeRageQueue(i);
                return true;
            }   
        }
        if (DEBUG) {
            Debug.Log("All cards are stable, removing none.");
        }
        return false;
    }

    public void removeRageQueue(int index) {
        rageQueue.RemoveAt(index);
        RageQueueDisplay.Instance.removeCard(index);
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

    public async void checkRage() {
        if (rage >= maxRage) {
            playerTurn = false;
            float speedMultipler = 1.0f;
            // Invoke will overflow into the next rage queue without this
            invoke = 0;
            while(rageQueue.Count > 0) {
                
                await Awaitable.WaitForSecondsAsync(0.6f/speedMultipler);
                int oldInvoke = invoke;
                invoke = 0;

                for (int i = 0; i < oldInvoke+1; i++) {
                    if (i > 0) {
                        RageQueueDisplay.Instance.resetDisplay(0, speedMultipler);
                        await Awaitable.WaitForSecondsAsync(0.05f/speedMultipler);
                    }

                    float sizeMuliplier = 1.00f + .02f*i;
                    if (sizeMuliplier > 2f) {
                        sizeMuliplier = 2f;
                    }

                    RageQueueDisplay.Instance.popDisplay(0, 1.1f*sizeMuliplier, speedMultipler);
                    await Awaitable.WaitForSecondsAsync(0.2f/speedMultipler);
                    rageQueue[0].triggerRage();

                    speedMultipler += 0.02f;
                    if (speedMultipler > 10f) {
                        speedMultipler = 10f;
                    }
                }
                
                rageQueue.RemoveAt(0);
                RageQueueDisplay.Instance.removeCard(0);
            }
            rage = 0;
            angrier = 0;
            playerTurn = true;
            RageQueueDisplay.Instance.updateDisplay();
        }

    }

    public void startTurn(){
        playerTurn = true;
        energy = maxEnergy;
        block = 0;
        drawCards(5);
        BoardManager.Instance.displayActions();
    }

    public void endTurn(){
        if(!playerTurn){
            return;
        }
        playerTurn = false;
        discardHand();
        BoardManager.Instance.executeActions();
    }

    void Update() {
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

        drawingCards = false;
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

        maxRageQueue = 10;

        invoke = 0;
        duplicate = 0;
        angrier = 0;
    }
}