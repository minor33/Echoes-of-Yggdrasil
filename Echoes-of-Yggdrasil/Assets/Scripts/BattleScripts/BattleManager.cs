using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public TestDeck testDeck;
    public Encounter encounter;
    private HandDisplay handDisplay;
    private BoardDisplay boardDisplay;
    //public Image background;
    private int battleState;
    private BattlePlayer player;
    public Enemy[] enemies;


    public void drawCard(){
        Card drawnCard = player.drawCard();
        handDisplay.addCard(drawnCard);
    }

    private async void startPlayerTurn() {
        for(int i = 0; i < 10; i++){
            await Awaitable.WaitForSecondsAsync(0.05f);
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
        boardDisplay = FindAnyObjectByType<BoardDisplay>(); 
        player = new BattlePlayer();

        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();

        //background.sprite = encounter.background;
        for(int i = 0; i < 3; i++){
            if(encounter.enemies[i] != null){
                boardDisplay.setEnemy(new Enemy(encounter.enemies[i]), i);
            }
        }

        battleState = 0;
    }
}
