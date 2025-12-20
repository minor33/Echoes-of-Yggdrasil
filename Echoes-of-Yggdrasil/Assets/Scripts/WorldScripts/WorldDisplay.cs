using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldDisplay : MonoBehaviour
{
    public World world;

    [SerializeField] private TMP_Text worldName;
    [SerializeField] private Image worldBackgroundImage;

    void Start()
    {
        worldName.text = world.name;
        worldBackgroundImage.color = world.color;
    }
}
