using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public TestDeck testDeck;
    public Encounter encounter;
    private HandDisplay handDisplay;
    private int battleState;
    private BattlePlayer player;
    public Enemy[] enemies;


    public void drawCard(){
        Card drawnCard = player.drawCard();
        handDisplay.addCard(drawnCard);
    }

    private async void startPlayerTurn() {
        for(int i = 0; i < 10; i++){
            await Awaitable.WaitForSecondsAsync(0.4f);
            drawCard();
        }
    }

    void Update() {
        if(battleState == 0){
            startPlayerTurn();
            battleState = 1;
        }
        // battleState=1 until player ends turn
        if(battleState == 2){
            // Start enemy turn
            battleState = 3;
        }
        // battleState=3 until enemy turn finishes
    }


    void Start() {
        handDisplay = FindAnyObjectByType<HandDisplay>(); 
        player = new BattlePlayer();

        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();

        enemies = new Enemy[3];
        for(int i = 0; i < 3; i++){
            if(encounter.enemies[i] != null){
                enemies[i] = new Enemy(encounter.enemies[i]);
            } else {
                enemies[i] = null;
            }
        }



        battleState = 0;
    }
}
