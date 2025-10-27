using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    public TestDeck testDeck;
    public Encounter encounter;
    private HandDisplay handDisplay;
    private BoardManager boardManager;
    private BattlePlayer player;
    //public Image background;
    private int battleState;

    public void progressBattleState(){
        battleState = (battleState+1)%4;
    }

    void Update() {
        if(battleState == 0){
            player.startTurn();
            boardManager.displayActions();
            battleState = 1;
        }
        // battleState=1 until player ends turn
        if(battleState == 2){
            // Start enemy turn
            battleState = 3;
            boardManager.executeActions();
        }
        // battleState=3 until enemy turn finishes
    }

    void Start() {
        handDisplay = HandDisplay.Instance;
        boardManager = BoardManager.Instance;
        player = BattlePlayer.Instance;

        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();

        for(int i = 0; i < 3; i++){
            if(encounter.enemies[i] != null){
                boardManager.setEnemy(encounter.enemies[i], i);
            }
        }

        battleState = 0;
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
    }
}
