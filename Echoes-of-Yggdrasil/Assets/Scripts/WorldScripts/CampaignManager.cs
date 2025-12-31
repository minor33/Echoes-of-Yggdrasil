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

    private List<Event> events;
    private Event[] activeEvents;
    [SerializeField] private GameObject eventPrefab;
    [SerializeField] private List<GameObject> eventSlots;

    private bool inEvent;
    [SerializeField] private GameObject eventPopup;
    [SerializeField] private TMP_Text eventPopupText;
    [SerializeField] private TMP_Text eventPopupTitle;

    [SerializeField] private Transform eventPopupChoices;
    [SerializeField] private GameObject choicePrefab;

    [SerializeField] private GameObject yggdrasilUI;
    [SerializeField] private GameObject worldUI;

    [SerializeField] private GameObject worldPrefab;
    private World[] worldList;

    public bool isInEvent(){
        return inEvent;
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
        inEvent = true;
        eventPopup.SetActive(true);
        eventPopupText.text = ev.text;
        eventPopupTitle.text = ev.name;
 
        float yPos = 0f;
        foreach(Choice choice in ev.choices){
            GameObject choiceDisplay = Instantiate(choicePrefab,eventPopupChoices.position, Quaternion.identity, eventPopupChoices.transform);
            choiceDisplay.GetComponent<ChoiceDisplay>().choice = choice;
            choiceDisplay.transform.localPosition = new Vector3(0,yPos,0);
            yPos -= 0.25f;
        }
    }

    public void nextEvent(int index){
        Event newEvent = events[0];
        events.RemoveAt(0);
        activeEvents[index] = newEvent;
        
        GameObject eventDisplay = Instantiate(eventPrefab, eventSlots[index].transform.position, Quaternion.identity, eventSlots[index].transform);
        eventDisplay.GetComponent<EventDisplay>().ev = newEvent;
    }

    void Start() {
        inEvent = false;
        act = 1;
        world = null;

        yggdrasilUI.SetActive(true);
        worldUI.SetActive(false);
        eventPopup.SetActive(false);
        
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
