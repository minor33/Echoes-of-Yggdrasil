using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public TestDeck testDeck;
    public Encounter encounter;

    public HandDisplay handDisplay;

    void Start() {
        BattlePlayer player = new BattlePlayer();

        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();

        player.drawCards(5);
    }
}
