using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    [SerializeField] private TestDeck testDeck;
    [SerializeField] private Encounter testEncounter;
    

    private HandDisplay handDisplay;
    private BoardManager boardManager;
    private BattlePlayer player;
    //public Image background;

    [SerializeField] private Button endTurnButton;
    
    public void startBattle(Encounter encounter){
        for(int i = 0; i < testDeck.cards.Count; i++){
            player.deck.Add(new Card(testDeck.cards[i]));
        }
        player.shuffleDeck();

        for(int i = 0; i < encounter.enemies.Length; i++){
            boardManager.setEnemy(encounter.enemies[i], i);
        }

        delayedStart(); // avoids making Start() async
    }

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

        if(CampaignManager.Instance == null){ // For testing, this is only true if game manually started from battle scene
            startBattle(testEncounter);
        } else {
            CampaignManager.Instance.enterBattle();
        }
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
