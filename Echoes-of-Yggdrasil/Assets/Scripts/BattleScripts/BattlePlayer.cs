using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private int maxRage;
    private int energy;
    private int maxEnergy;
    private int maxRageQueue;

    private int invoke;
    private int duplicate;
    private int rageAdjustment;
    private int skip;
    private int expand;

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

    // Adds additonal space in the rage queue temporarily
    public void addExpand(int e) {
        expand += e;
        addMaxRageQueue(e);
    }

    // Additional damage on rage queue cards
    // This should probably change to a general damage modifier so it can go negative too
    public void addRageAdjustment(int r) {
        rageAdjustment += r;
    }

    public void addMaxRageQueue(int increase) {
        maxRageQueue += increase;
        // Only spot that the largest queue size is defined, except in RageQueueDisplay
        maxRageQueue = Math.Max(Math.Min(maxRageQueue, 20), 0);
        RageQueueDisplay.Instance.updateDisplay();
    }

    // Gives the next X cards in the rage queue -1 trigger
    // IMPORTANT INTERACTION: Invoke 5 Skip 1 gives the next cards 5 total triggers, and the card after its normal 1
    public void addSkip(int s) {
        skip += s;
    }

    // Selection Helpers

    public void startSelection() {
        playerTurn = false;
        selectingRageCards = true;
    }

    public void endSelection() {
        var display = RageQueueDisplay.Instance;
        display.updateDisplay();
        selectingRageCards = false;
        playerTurn = true;
    }

    public async Task<List<int>> getSelection(int num_cards) {
        if (rageQueue.Count < num_cards) {
            Debug.LogError("Too few cards in rage queue for GetSelection(): Do non erroneous checking before using this function");
            return new List<int>();
        }

        while (true) {
            if (selectedCards.Count >= num_cards) {
                List<int> indices = new List<int>();
                for (int i = 0; i < rageQueue.Count; i++) {
                    RageCardInteraction card = rageQueue[i].GetComponent<RageCardInteraction>();
                    if (selectedCards.Contains(card)) {
                        indices.Add(i);
                    }
                }
                return indices;
            }
            await Awaitable.WaitForSecondsAsync(.1f);
        }
    }

    public void deselectAll() {
        while (selectedCards.Count > 0) {
            selectedCards[0].deselect();
        }
    }

    public void shuffleDeck() {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
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
            rageQueueRetain.Add(playedCard.getRageAbility().getKeywordValue(RETAIN));
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
                removeRageCard(i);
                return true;
            }   
        }
        if (DEBUG) {
            Debug.Log("All cards are stable, removing none.");
        }
        return false;
    }

    public void removeRageCard(int index) {
        Destroy(rageQueue[index]);
        rageQueue.RemoveAt(index);
        rageQueueRetain.RemoveAt(index);
    }

    public void reverseRageQueue() {
        rageQueue.Reverse();
        rageQueueRetain.Reverse();
        RageQueueDisplay.Instance.updateDisplay();
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
            int totalTriggers = 0;
            // Invoke will overflow into the next rage queue without this
            invoke = 0;
            
            while(rageQueue.Count > triggerCard) {
                
                await Awaitable.WaitForSecondsAsync(0.6f/speedMultipler);
                Card card = getRageCard(triggerCard);
                // INVOKE
                int triggers = invoke+1;
                int repeat = 0;
                int starter = 0;
                int finisher = 0;
                int patient = 0;
                

                // SKIP
                if (skip > 0) {
                    triggers -= 1;
                }
                // So if we're being so real, REPEAT, STARTER, FINISHER and other forms of repeat should be in the abilities trigger and not here
                // That would make them work in play effects, although that only applies to repeat. Maybe repeat should just be moved into Ability?
                // The multiplication would still work properly. We can cross that bridge when we want a play effect to have repeat I guess. I just feel
                // bad about splitting the repeat effects across multiple places. 
                // REPEAT
                repeat = card.getRageAbility().getKeywordValue(REPEAT);
                triggers += repeat*triggers;

                // STARTER
                if (totalCardTriggers == 0) {
                    starter = card.getRageAbility().getKeywordValue(STARTER);
                    triggers += starter*triggers;
                }

                // FINISHER: Should only ever be equal, and never less than
                if (rageQueue.Count <= triggerCard+1) {
                    finisher = card.getRageAbility().getKeywordValue(FINISHER);
                    triggers += finisher*triggers;
                }

                // PATIENT
                if (card.getRageAbility().hasKeyword(PATIENT)) {
                    patient = totalTriggers;
                    triggers += triggers*patient; // Yeah, this is going to be a problem. Triggers go brrrrr
                }

                if (DEBUG) {
                    Debug.Log($"{card} is triggering {triggers} time(s) in the rage queue with: {invoke} - invoke | {skip} - skip | {repeat} - repeat | {starter} - starter | {finisher} - finisher | {patient} - patient");
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
                    totalTriggers += triggers;
                }
                
                if (rageQueueRetain[triggerCard] > 0) {
                    if (DEBUG) {
                        Debug.Log($"Retaining {card} with {rageQueueRetain[triggerCard]} retain");
                    }
                    RageQueueDisplay.Instance.resetDisplay(triggerCard, speedMultipler);
                    rageQueueRetain[triggerCard] --;
                    triggerCard += 1;
                } else {
                    removeRageCard(triggerCard);
                }
            }
            rage = 0;
            rageAdjustment = 0;
            maxRageQueue -= expand;
            expand = 0;
            RageQueueDisplay.Instance.updateDisplay();
            playerTurn = true;
            
        }

    }

    public void swapRageCard(int index1, int index2) {
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

    public async void swapRageCardSelection(int swaps) {
        if (rageQueue.Count < 2) {
            Debug.Log("Not Swapping Cards: Too Few in Rage Queue");
            return;
        }

        startSelection();

        for (int _ = 0; _ < swaps; _++) {
            Debug.Log("Swapping");

            List<int> selection = await getSelection(2);
            
            swapRageCard(selection[0], selection[1]);
            deselectAll();
            RageQueueDisplay.Instance.updateDisplay();
        }

        Debug.Log("Done Swapping");
        endSelection();
    }
    
    public async void removeRageCardSelection(int removals) {
        if (rageQueue.Count < 1) {
            Debug.Log("Not Swapping Cards: Too Few in Rage Queue");
            return;
        }

        removals = Math.Min(removals, rageQueue.Count);

        startSelection();

        for (int _ = 0; _ < removals; _++) {
            Debug.Log("Removing");

            List<int> selection = await getSelection(1);
            
            removeRageCard(selection[0]);
            deselectAll();
            RageQueueDisplay.Instance.updateDisplay();
        }

        Debug.Log("Done Removing");
        endSelection();
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