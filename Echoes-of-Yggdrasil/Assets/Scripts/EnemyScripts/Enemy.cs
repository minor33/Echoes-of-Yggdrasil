using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static EnemyAction;

public class Enemy : Unit {
    public EnemyData enemyData;

    private int currentAction;

    public Image enemyImage;
    public TMP_Text nextActionText;
    public Image nextActionImage;

    public Sprite attackActionSprite;
    public Sprite defendActionSprite;
    public Sprite pauseActionSprite;

    public async void executeAction(){
        ActionPair actionPair = enemyData.actionPairs[currentAction];
        switch (actionPair.action) {
            case ATTACK:
                BattlePlayer.Instance.damage(actionPair.attack);
                Vector3 originalPosition = transform.localPosition;
                Vector3 newPosition = originalPosition + new Vector3(0, -0.5f, 0);
                transform.DOLocalMove(newPosition, 0.12f);
                await Awaitable.WaitForSecondsAsync(0.15f);
                transform.DOLocalMove(originalPosition, 0.20f);
                break;
            case DEFEND:
                gainBlock(actionPair.defend);
                break;
            case PAUSE:
                break;
            default:
                break;
        }
        nextActionImage.enabled = false;
        nextActionText.enabled = false;
        currentAction = (currentAction+1) % enemyData.actionPairs.Count;
    }

    public void displayAction(){
        nextActionImage.enabled = true;
        nextActionText.enabled = false;

        ActionPair actionPair = enemyData.actionPairs[currentAction];

        switch (actionPair.action) {
            case ATTACK:
                nextActionImage.sprite = attackActionSprite;
                nextActionImage.transform.localPosition = new Vector3(-0.11f,0,0);
                nextActionText.enabled = true;
                nextActionText.text = $"{actionPair.attack}";
                break;
            case DEFEND:
                nextActionImage.sprite = defendActionSprite;
                nextActionImage.transform.localPosition = new Vector3(-0.11f,0,0);
                nextActionText.enabled = true;
                nextActionText.text = $"{actionPair.defend}";
                break;
            case PAUSE:
                nextActionImage.sprite = pauseActionSprite;
                nextActionImage.transform.localPosition = new Vector3(0,0,0);
                break;
            default:
                break;
        }

    }

    public void updateDisplay() {
        updateHealthbar();
    }

    void Start() {
        maxHealth = enemyData.maxHealth;
        health = maxHealth;
        currentAction = Random.Range(0, enemyData.actionPairs.Count);

        enemyImage.sprite = enemyData.sprite;
        nextActionText.enabled = false;
        nextActionImage.enabled = false;

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
