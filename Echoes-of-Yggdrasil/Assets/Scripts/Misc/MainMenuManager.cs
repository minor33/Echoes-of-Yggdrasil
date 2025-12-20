using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject deitySelectUI;
    [SerializeField] private GameObject deityPrefab;
    [SerializeField] private Button confirmSelectionButton;

    private Deity[] selectedDeities;

    public void unselectDeity(Deity deity){
        for(int i = 0; i < 2; i++){
            if(selectedDeities[i] == deity){
                selectedDeities[i] = null;
                confirmSelectionButton.interactable = false;
                return;
            }
        }
        Debug.LogError("NO DEITY FOUND TO UNSELECT");
    }

    public bool selectDeity(Deity deity){ // Returns false if operation failed 
        if(selectedDeities[0] == null || selectedDeities[1] == null){
            if(selectedDeities[0] == null){
                selectedDeities[0] = deity;
            } else {
                selectedDeities[1] = deity;
            }
            if(selectedDeities[0] != null && selectedDeities[1] != null){
                confirmSelectionButton.interactable = true;
            }
            return true;
        } else {
            return false;
        }
    }

    public void deitySelection() {
        homeUI.SetActive(false);
        deitySelectUI.SetActive(true);

        Deity[] allDeities = Resources.LoadAll<Deity>("Deities");

        float xPos = -3f;
        float xSpacing = 1.1f;
        for(int i = 0; i < allDeities.Length; i++){
            GameObject deityDisplay = Instantiate(deityPrefab, deitySelectUI.transform.position, Quaternion.identity, deitySelectUI.transform);
            deityDisplay.GetComponent<DeityDisplay>().deity = allDeities[i];
            deityDisplay.transform.localPosition = new Vector3(xPos,0.75f,0);
            xPos += xSpacing;
        }

        selectedDeities = new Deity[2];
        confirmSelectionButton.interactable = false;
    }

    public void startNewRun(){
        SceneManager.LoadScene("CampaignScene");
    }

    void Start() {
        homeUI.SetActive(true);
        deitySelectUI.SetActive(false);
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
