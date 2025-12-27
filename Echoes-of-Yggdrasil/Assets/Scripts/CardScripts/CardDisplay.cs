using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    
    public TMP_Text nameText;
    public TMP_Text energyText;
    public TMP_Text rageText;
    public TMP_Text playAbilityDescriptionText;
    public TMP_Text rageAbilityDescriptionText;
    public TMP_Text retainText;

    public Image cardGlow;

    public int retain;

    public void updateDisplay() {
        nameText.text = card.getName();
        energyText.text = $"{card.getEnergy()}";
        int rage = card.getRage();
        rageText.text = $"{rage}";
        if (rage == 99) { // 99 Represents instant trigger
            rageText.text = "!";
        }
        
        if (playAbilityDescriptionText != null) {
            playAbilityDescriptionText.text = card.getPlayAbilityDescription();
        }
        rageAbilityDescriptionText.text = card.getRageAbilityDescription();

        if (retainText != null) { 
            if (retain > 0) {
                retainText.enabled = true;
                retainText.text = $"{retain}";
            } else {
                retainText.enabled = false;
            }
        }
    }

    void Start() {
        cardGlow.enabled = false;
        if (retainText != null) {
            retainText.enabled = false;
        }
        updateDisplay();
    }
}
