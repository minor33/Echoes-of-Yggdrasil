using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using EditorAttributes;

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

    public int getMaxRageQueue() {
        return maxRageQueue;
    }

    public void addInvoke(int i) {
        invoke += i;
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
        addCardToRageQueue(playedCard);
        discard.Add(playedCard);
        BattlePlayer.Instance.checkRage();
    }

    public void addCardToRageQueue(Card playedCard) {
        if (rageQueue.Count >= maxRageQueue) {
            // Stable goes here
            removeRageQueue(0);
        }
        rageQueue.Add(playedCard);
        RageQueueDisplay.Instance.addCard(playedCard);
    }

    public void removeCard(int index){
        hand.RemoveAt(index);
        HandDisplay.Instance.removeCard(index);
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
            while(rageQueue.Count > 0) {
                await Awaitable.WaitForSecondsAsync(0.6f);
                int oldInvoke = invoke;
                invoke = 0;

                for (int i = 0; i < oldInvoke+1; i++) {
                    if (i > 0) {
                        RageQueueDisplay.Instance.resetDisplay(0);
                        await Awaitable.WaitForSecondsAsync(0.05f);
                    }

                    RageQueueDisplay.Instance.popDisplay(0, 1.1f + 0.05f*i);
                    await Awaitable.WaitForSecondsAsync(0.2f);
                    rageQueue[0].triggerRage();
                }
                
                rageQueue.RemoveAt(0);
                RageQueueDisplay.Instance.removeCard(0);
            }
            rage = 0;
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
    }
}