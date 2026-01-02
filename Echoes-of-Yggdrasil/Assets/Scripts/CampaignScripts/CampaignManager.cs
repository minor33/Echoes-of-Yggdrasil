using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance { get; private set; }

    private int act;
    private World world;

    [SerializeField] private GameObject worldPrefab;
    private World[] worldList;

    private List<Event> events;
    private Event[] activeEvents;
    private GameObject[] activeEventDisplays;
    [SerializeField] private GameObject eventPrefab;
    [SerializeField] private List<GameObject> eventSlots;
    private Event currentEvent;
    [SerializeField] private GameObject eventPopup;
    [SerializeField] private GameObject eventManager;
    [SerializeField] private TMP_Text eventPopupText;
    [SerializeField] private TMP_Text eventPopupTitle;
    [SerializeField] private Transform eventPopupChoices;
    [SerializeField] private GameObject choicePrefab;
    private List<GameObject> choiceDisplayList;

    [SerializeField] private GameObject yggdrasilUI;
    [SerializeField] private GameObject worldUI;

    private int silver;
    [SerializeField] private TMP_Text silverNumber;    

    public void adjustSilver(int adjustment){
        silver += adjustment;
        silverNumber.text = $"{silver}";
    }

    public bool inEvent(){
        if(currentEvent == null){
            return false;
        } else {
            return true;
        }
    }

    public Event getCurrentEvent(){
        return currentEvent;
    }

    public void displayNextWorlds(){
        List<World> actWorlds = new List<World>();
        foreach(World world in worldList){
            if(world.worldTier == act){
                actWorlds.Add(world);
            }
        }
        actWorlds.RemoveAt(UnityEngine.Random.Range(0,3));

        float xPos = -0.9f;
        float yPos = -1.35f;
        yPos += (act-1)*0.5f;
        for(int i = 0; i < 2; i++){
            GameObject worldDisplay = Instantiate(worldPrefab, yggdrasilUI.transform.position, Quaternion.identity, yggdrasilUI.transform);
            worldDisplay.GetComponent<WorldDisplay>().world = actWorlds[i];
            worldDisplay.transform.localPosition = new Vector3(xPos,yPos,0);
            xPos += xPos*-2;
        }
    }

    public void enterWorld(World world){
        this.world = world;
        events = new List<Event>();
        foreach(Event ev in world.events){
            events.Add(ev);
        }
        activeEvents = new Event[3];
        activeEventDisplays = new GameObject[3];
        
        // Shuffle events
        int n = events.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Event value = events[k];
            events[k] = events[n];
            events[n] = value;
        }

        yggdrasilUI.SetActive(false);
        worldUI.SetActive(true);

        for(int i = 0; i < 3; i++){
            nextEvent(i);
        }
    }

    public void enterEvent(Event ev){
        eventPopup.SetActive(true);
        eventManager.SetActive(false);
        currentEvent = ev;
        eventPopupText.text = ev.text;
        eventPopupTitle.text = ev.name;
 
        float yPos = 0f;
        foreach(Choice choice in ev.choices){
            GameObject choiceDisplay = Instantiate(choicePrefab,eventPopupChoices.position, Quaternion.identity, eventPopupChoices);
            choiceDisplay.GetComponent<ChoiceDisplay>().choice = choice;
            choiceDisplay.transform.localPosition = new Vector3(0,yPos,0);
            choiceDisplayList.Add(choiceDisplay);
            yPos -= 0.25f;
        }
    }

    public void exitEvent(){
        foreach(GameObject cd in choiceDisplayList){
            Destroy(cd);
        }
        choiceDisplayList.Clear();
        currentEvent = null;
        eventPopup.SetActive(false);
        eventManager.SetActive(true);
    }

    public void nextEvent(int index){
        if(activeEvents[index] != null){
            Destroy(activeEventDisplays[index]);
            activeEventDisplays[index] = null;
            activeEvents[index] = null;
        }

        if(events.Count == 0){
            return;
        }

        Event newEvent = events[0];
        events.RemoveAt(0);
        activeEvents[index] = newEvent;

        GameObject eventDisplay = Instantiate(eventPrefab, eventSlots[index].transform.position, Quaternion.identity, eventSlots[index].transform);
        eventDisplay.GetComponent<EventDisplay>().ev = newEvent;
        activeEventDisplays[index] = eventDisplay;
    }

    void Start() {
        currentEvent = null;
        act = 1;
        world = null;
        
        silver = 0;
        silverNumber.text = $"{silver}";

        yggdrasilUI.SetActive(true);
        worldUI.SetActive(false);
        eventPopup.SetActive(false);

        choiceDisplayList = new List<GameObject>();
        
        worldList = Resources.LoadAll<World>("Worlds");
        displayNextWorlds();
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
