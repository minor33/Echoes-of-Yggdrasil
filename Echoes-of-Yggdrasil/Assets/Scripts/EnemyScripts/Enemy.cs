using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using static EnemyAction;

public class Enemy : Unit {
    EnemyData enemyData;

    public int currentAction;

    public Enemy(EnemyData enemyData) {
        this.enemyData = enemyData;
        maxHealth = enemyData.maxHealth;
        health = maxHealth;
        currentAction = Random.Range(0, enemyData.actionPairs.Count);
    }

    public int getHealth() {
        return health;
    }
    public int getMaxHealth() {
        return maxHealth;
    }
    public Sprite getSprite() {
        return enemyData.sprite;
    }

    public override void die() {
        return;
        //To do??
    }
}
