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
        int health = enemy.getHealth();
        int maxHealth = enemy.getMaxHealth();
        healthText.text = $"{health}/{maxHealth}";
        healthBarFill.fillAmount = health / maxHealth;
    }

    void Start() {
        enemyImage.sprite = enemy.getSprite();
        nextAttackText.enabled = false;
        nextAttackImage.enabled = false;

        updateDisplay();
    }

    // Could be optimized
    void Update() {
        updateDisplay();
    }
}
