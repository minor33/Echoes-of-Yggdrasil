using UnityEngine;
using System.Collections;
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
    public int rageAdjustment;
    public int skip;

    private bool drawingCards;
    private bool playerTurn;

    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;
    public List<GameObject> rageQueue;
    public List<int> rageQueueRetain;  // Retain could be moved into CardDisplay. Would be convient when working on the retain HUD
    public List<RageCardInteraction> selectedCards;
    public bool selectingRageCards;
    public GameObject rageCardPrefab;

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

    public int getRageAdjustment() {
        return rageAdjustment;
    }

    public int getMaxRageQueue() {
        return maxRageQueue;
    }

    public Card getRageCard(int index) {
        return rageQueue[index].GetComponent<CardDisplay>().card;
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
    public void addRageAdjustment(int r) {
        rageAdjustment += r;
    }

    // Gives the next X cards in the rage queue -1 trigger
    // IMPORTANT INTERACTION: Invoke 5 Skip 1 gives the next cards 5 total triggers, and the card after its normal 1
    public void addSkip(int s) {
        skip += s;
    }

    public void deselectAll() {
        while (selectedCards.Count > 0) {
            selectedCards[0].deselect();
        }
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

    public async void playCard(int index, Enemy targetEnemy = null) {
        Card playedCard = hand[index];
        energy -= playedCard.getEnergy();
        removeCard(index);
        playedCard.play(targetEnemy);

        while (!playerTurn) {
            await Awaitable.WaitForSecondsAsync(0.1f);
        }
        
        if (!playedCard.getPlayAbility().hasKeyword(TACTICAL)) {
            for (int i = 0; i < 1+duplicate; i++) {
                addCardToRageQueue(playedCard);
            }
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
            // CardDisplay needs to be created in RageQueueDisplay but then stored in BattlePlayer
            RageQueueDisplay display = RageQueueDisplay.Instance;
            GameObject cardDisplay = Instantiate(rageCardPrefab, display.transform.position, Quaternion.identity, display.transform);
            cardDisplay.GetComponent<CardDisplay>().card = playedCard;
            // cardDisplay.transform.localScale = Vector3.zero;
            rageQueue.Add(cardDisplay);
            rageQueueRetain.Add(playedCard.getRageAbility().getRetain());
            RageQueueDisplay.Instance.updateDisplay();
        }
    }

    public void removeCard(int index){
        hand.RemoveAt(index);
        HandDisplay.Instance.removeCard(index);
    }

    public bool removeFrontRageQueue() {
        for (int i = 0; i < rageQueue.Count; i++) {
            if (!getRageCard(i).getRageAbility().hasKeyword(STABLE)) {
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
        Destroy(rageQueue[index]);
        rageQueue.RemoveAt(index);
        rageQueueRetain.RemoveAt(index);
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
            int triggerCard = 0;
            int totalCardTriggers = 0;
            // Invoke will overflow into the next rage queue without this
            invoke = 0;
            
            while(rageQueue.Count > triggerCard) {
                
                await Awaitable.WaitForSecondsAsync(0.6f/speedMultipler);
                Card card = getRageCard(triggerCard);
                // INVOKE
                int triggers = invoke+1;
                int starter = 0;
                int finisher = 0;

                // SKIP
                if (skip > 0) {
                    triggers -= 1;
                }
                // STARTER
                if (totalCardTriggers == 0) {
                    starter = card.getRageAbility().getStarter();
                    triggers += starter*triggers;
                }

                // FINISHER: Should only ever be equal, and never less than
                if (rageQueue.Count <= triggerCard+1) {
                    finisher = card.getRageAbility().getFinisher();
                    triggers += finisher*triggers;
                }

                if (DEBUG) {
                    Debug.Log($"{card} is triggering {triggers} time(s) in the rage queue with: {invoke} - invoke | {skip} - skip | {starter} - starter | {finisher} - finisher");
                }

                if (skip > 0) {
                    skip --;
                }
                invoke = 0;
                for (int i = 0; i < triggers; i++) {
                    if (i > 0) {
                        RageQueueDisplay.Instance.resetDisplay(0, speedMultipler);
                        await Awaitable.WaitForSecondsAsync(0.05f/speedMultipler);
                    }

                    float sizeMuliplier = 1.00f + .02f*i;
                    if (sizeMuliplier > 2f) {
                        sizeMuliplier = 2f;
                    }

                    RageQueueDisplay.Instance.popDisplay(triggerCard, 1.1f*sizeMuliplier, speedMultipler);
                    await Awaitable.WaitForSecondsAsync(0.2f/speedMultipler);
                    card.triggerRage();

                    speedMultipler += 0.02f;
                    if (speedMultipler > 10f) {
                        speedMultipler = 10f;
                    }
                }
                

                // SKIP animation: Could be different? 
                if (triggers <= 0) {
                    RageQueueDisplay.Instance.popDisplay(triggerCard, 0.8f);
                    await Awaitable.WaitForSecondsAsync(0.2f);
                } else {
                    totalCardTriggers ++;
                }
                
                if (rageQueueRetain[triggerCard] > 0) {
                    if (DEBUG) {
                        Debug.Log($"Retaining {card} with {rageQueueRetain[triggerCard]} retain");
                    }
                    RageQueueDisplay.Instance.resetDisplay(triggerCard, speedMultipler);
                    rageQueueRetain[triggerCard] --;
                    triggerCard += 1;
                } else {
                    removeRageQueue(triggerCard);
                }
            }
            rage = 0;
            rageAdjustment = 0;
            playerTurn = true;
            RageQueueDisplay.Instance.updateDisplay();
        }

    }

    public void swap(int index1, int index2) {
        if (index1 <= -1 || index2 <= -1 || index1 >= rageQueue.Count || index2 >= rageQueue.Count) {
            Debug.LogError($"Indicies given to swap invalid: {index1} and {index2}");
            return;
        }
        Debug.Log($"Swapping Cards in Rage Queue: {index1} and {index2}");
        var temp = rageQueue[index1];
        var tempRetain = rageQueueRetain[index1];
        rageQueue[index1] = rageQueue[index2];
        rageQueueRetain[index1] = rageQueueRetain[index2];
        rageQueue[index2] = temp;
        rageQueueRetain[index2] = tempRetain;
    }

    public async void swapRageCard(int swaps) {
        if (rageQueue.Count < 2) {
            Debug.Log("Not Swapping Cards: Too Few in Rage Queue");
            return;
        }

        playerTurn = false;
        var display = RageQueueDisplay.Instance;
        selectingRageCards = true;

        for (int _ = 0; _ < swaps; _++) {
            Debug.Log("Swapping");
            
            while (true) {
                if (selectedCards.Count >= 2) {
                    var indices = (Index1: -1, Index2: -1);
                    var oneFound = false;
                    for (int i = 0; i < rageQueue.Count; i++) {
                        RageCardInteraction card = rageQueue[i].GetComponent<RageCardInteraction>();
                        if (card == selectedCards[0].GetComponent<RageCardInteraction>() ||
                            card == selectedCards[1].GetComponent<RageCardInteraction>()) {
                            if (!oneFound) {
                                indices.Index1 = i;
                                oneFound = true;
                            } else {
                                indices.Index2 = i;
                            }
                        }
                    }
                    swap(indices.Index1, indices.Index2);
                    deselectAll();
                    break;
                }
                await Awaitable.WaitForSecondsAsync(.1f);
            }
            display.updateDisplay();
            Debug.Log("Done Swapping");
        }
        
        selectingRageCards = false;
        playerTurn = true;
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
        rageQueue = new List<GameObject>();
        rageQueueRetain = new List<int>();

        maxHealth = 100;
        health = maxHealth;

        maxRage = 10;
        rage = 0;

        maxEnergy = 3;
        energy = 3;

        maxRageQueue = 10;

        invoke = 0;
        duplicate = 0;
        rageAdjustment = 0;
    }
}