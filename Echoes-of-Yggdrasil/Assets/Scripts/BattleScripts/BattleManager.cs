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

    public Button endTurnButton;
    
    private async void delayedStart(){
        await Awaitable.WaitForSecondsAsync(0.01f); // Gives enemies time to load their data
        player.startTurn();
    }

    void Update() {
        if(player.isPlayerTurn()){
            endTurnButton.interactable = true;
        } else {
            endTurnButton.interactable = false;
        }
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

        delayedStart(); // avoids making Start() async
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
