using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance { get; private set; }

    private int act;

    [SerializeField] private GameObject yggdrasilUI;

    private World[] worldList;

    private void displayNextWorlds(){
        List<World> actWorlds = new List<World>();
        foreach(World world in worldList){
            if(world.worldTier == act){
                actWorlds.Add(world);
            }
        }
        actWorlds.RemoveAt(UnityEngine.Random.Range(0,3));


    }

    void Start() {
        act = 1;
        yggdrasilUI.SetActive(true);
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
