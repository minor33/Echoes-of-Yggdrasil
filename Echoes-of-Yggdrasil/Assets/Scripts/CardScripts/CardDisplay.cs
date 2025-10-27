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

    public Image cardGlow; 

    public void updateDisplay() {
        nameText.text = card.getName();
        energyText.text = $"{card.getEnergy()}";
        rageText.text = $"{card.getRage()}";
        if (playAbilityDescriptionText != null) {
            playAbilityDescriptionText.text = card.getPlayAbilityDescription();
        }
        rageAbilityDescriptionText.text = card.getRageAbilityDescription();
    }

    void Start() {
        cardGlow.enabled = false;
        updateDisplay();
    }
}
