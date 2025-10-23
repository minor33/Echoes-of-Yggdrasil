using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public TestDeck testDeck;
    //public Encounter encounter;

    private HandDisplay handDisplay;

    public void drawCard(BattlePlayer player){
        Card drawnCard = player.drawCard();
        handDisplay.addCard(drawnCard);
    }

    async void Start() {
        handDisplay = FindAnyObjectByType<HandDisplay>(); 
        BattlePlayer player = new BattlePlayer();

        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();
  
        for(int i = 0; i < 10; i++){
            await Awaitable.WaitForSecondsAsync(0.4f);
            drawCard(player);
        }

    }
}
