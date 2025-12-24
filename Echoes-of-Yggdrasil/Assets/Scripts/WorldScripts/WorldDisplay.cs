using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldDisplay : MonoBehaviour, IPointerDownHandler
{
    public World world;

    [SerializeField] private TMP_Text worldName;
    [SerializeField] private Image worldBackgroundImage;

    void Start()
    {
        worldName.text = world.name;
        worldBackgroundImage.color = world.color;
    }

    public void OnPointerDown(PointerEventData eventData){
        CampaignManager.Instance.enterWorld(world);
    }
}
