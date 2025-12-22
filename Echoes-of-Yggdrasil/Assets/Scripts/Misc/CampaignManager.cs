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
    [SerializeField] private GameObject worldPrefab;

    private World[] worldList;

    private void displayNextWorlds(){
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
