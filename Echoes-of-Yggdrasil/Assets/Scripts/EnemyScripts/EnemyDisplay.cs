using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;
    
    public Image enemyImage;
    public TMP_Text healthText;
    public TMP_Text nextAttackText;
    public Image healthBarFill;
    public Image nextAttackImage;

    public void updateDisplay() {
        healthText.text = $"{enemy.getHealth()}/{enemy.getMaxHealth()}";
    }

    void Start() {
        updateDisplay();
    }
}
