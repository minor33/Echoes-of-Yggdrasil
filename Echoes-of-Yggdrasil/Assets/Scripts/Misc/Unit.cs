using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public abstract class Unit : MonoBehaviour {
    protected int maxHealth;
    protected int health;
    protected int block;

    public TMP_Text healthbarText;
    public Image healthbarFill;
    public GameObject healthbarBlock;
    public TMP_Text blockText;

    public void damage(int damage) {
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
}