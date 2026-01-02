using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceHandler : MonoBehaviour
{
    public static ChoiceHandler Instance { get; private set; }

    private CampaignManager cm;

    void Start(){
        cm = CampaignManager.Instance;
    }

    public void makeChoice(string eventName, int choiceIndex){
        bool endEvent = true;
        switch(eventName){
            case "Avalanche" :
                switch(choiceIndex){
                    case 1:
                        cm.adjustSilver(10);
                        break;
                    case 2:
                        cm.adjustSilver(20);
                        break;
                    case 3:
                        cm.adjustSilver(30);
                        break;
                }
                break;
            case "Goblin Ambush" :
                cm.enterEncounter("GoblinAmbush");
                break;
            default :
                Debug.LogError("CHOICE HANDLER CANNOT FIND EVENT");
                break;
        }
        if(endEvent){
            cm.exitEvent();
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
