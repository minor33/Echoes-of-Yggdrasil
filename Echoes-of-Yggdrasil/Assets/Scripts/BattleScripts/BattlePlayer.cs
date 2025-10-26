using UnityEngine;
using System.Collections.Generic;

public class BattlePlayer : Unit {
    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;

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

    public Card drawCard() {
        if(deck.Count == 0){
            reshuffleDiscard();
            if(deck.Count == 0){
                Debug.Log("Hand empty");
                return null;
            }
        }
        Card drawnCard = deck[0];
        hand.Add(drawnCard);
        deck.RemoveAt(0);
        return drawnCard;
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

    public BattlePlayer() {
        hand = new List<Card>();
        deck = new List<Card>();
        discard = new List<Card>();

        maxHealth = 100;
        health = maxHealth;
    }
}