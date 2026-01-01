using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using static GameConstants;
using static StatusKeyword;
using static StatusEffect;


public abstract class Unit : MonoBehaviour {
    protected int maxHealth;
    protected int health;
    protected int block;

    protected List<StatusEffect> statusEffects;

    public TMP_Text healthbarText;
    public Image healthbarFill;
    public GameObject healthbarBlock;
    public TMP_Text blockText;

    public void damage(int damage) {
        if (damage < 0) {
            if (DEBUG) {
                Debug.Log("Negative amount of damage done, leaving.");
            }
            return;
        }
        block -= damage;
        if(block < 0){
            health += block;
            if (health <= 0) {
                health = 0;
                die();
            }
            block = 0;
        }
    }

    public void gainBlock(int block) {
        this.block += block;
    }

    // Adds a new status effect to a unit, or adds the existing value to the current value
    public void addStatusEffect(StatusKeyword keyword, int value=1) {
        if (DEBUG) {
            Debug.Log($"Adding {keyword} effect of value {value} to {this}");
        }
        for (int i = 0; i < statusEffects.Count; i++) {
            if (statusEffects[i].keyword == keyword) {
                statusEffects[i].value += value;
                return;
            }
        }
        statusEffects.Add(new StatusEffect(keyword, value));
    }

    public int getStatusEffect(StatusKeyword keyword) {
        foreach (StatusEffect se in statusEffects) {
            if (se.keyword == keyword) {
                return se.value;
            }
        }
        return 0;
    }

    public void updateHealthbar() {
        healthbarText.text = $"{health}/{maxHealth}";
        healthbarFill.fillAmount = (float)health / (float)maxHealth;
        if(block > 0){
            healthbarBlock.SetActive(true);
            blockText.text = $"{block}";
            healthbarFill.color = new Color32(99,198,213,255);
        } else {
            healthbarBlock.SetActive(false);
            healthbarFill.color = new Color32(121,194,40,255);
        }
    }

    public abstract void die();

    protected virtual void Awake() {
        statusEffects = new List<StatusEffect>();
    }
}