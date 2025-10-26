using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using static EnemyAction;

public class Enemy : Unit {
    public EnemyData enemyData;

    private int currentAction;

    public Image enemyImage;
    public TMP_Text healthText;
    public TMP_Text nextAttackText;
    public Image healthBarFill;
    public Image nextAttackImage;

    public void updateDisplay() {
        healthText.text = $"{health}/{maxHealth}";
        healthBarFill.fillAmount = (float)health / (float)maxHealth;
    }

    void Start() {
        maxHealth = enemyData.maxHealth;
        health = maxHealth;
        currentAction = Random.Range(0, enemyData.actionPairs.Count);

        enemyImage.sprite = enemyData.sprite;
        nextAttackText.enabled = false;
        nextAttackImage.enabled = false;

        updateDisplay();
    }

    // Could be optimized
    void Update() {
        updateDisplay();
    }

    public int getHealth() {
        return health;
    }
    public int getMaxHealth() {
        return maxHealth;
    }

    public override void die() {
        Destroy(gameObject);
    }
}
