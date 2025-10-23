using UnityEngine;
using System.Collections.Generic;

public class BattlePlayer {
    public List<Card> hand;
    public List<Card> deck;
    public List<Card> discard;

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

    public void drawCards(int num) {
        for(int i = 0; i < num; i++){
            if(deck.Count == 0){
                return;
            }
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    public void debugPrintHand() {
        string message = "Hand: ";
        for(int i = 0; i < hand.Count; i++){
            message += $"{hand[i].getName()}, ";
        }
        Debug.Log(message);
    }

    public BattlePlayer() {
        hand = new List<Card>();
        deck = new List<Card>();
        discard = new List<Card>();
    }
}