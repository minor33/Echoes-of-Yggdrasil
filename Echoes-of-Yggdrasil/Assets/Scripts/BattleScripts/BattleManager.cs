using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    public TestDeck testDeck;
    public Encounter encounter;
    private HandDisplay handDisplay;
    private BoardDisplay boardDisplay;
    //public Image background;
    private int battleState;
    private BattlePlayer player;
    public Enemy[] enemies;


    public Enemy getEnemy(int index = 0) {
        for(int i = 0; i < 3; i++) { 
            // Change > 0 line
            if (enemies[(index+i)%3] != null && enemies[(index+i)%3].getHealth() > 0) {
                return enemies[(index+i)%3];
            }
        }
        return null;
    }

    public BattlePlayer getPlayer() {
        return player;
    }

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
        enemies = new Enemy[3];
        for(int i = 0; i < 3; i++){
            if(encounter.enemies[i] != null){
                Enemy newEnemy = new Enemy(encounter.enemies[i]);
                boardDisplay.setEnemy(newEnemy, i);
                enemies[i] = newEnemy;
            }
        }

        battleState = 0;
    }

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
