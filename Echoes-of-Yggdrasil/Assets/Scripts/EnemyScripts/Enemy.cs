using UnityEngine;
using System.Collections.Generic;
using static EnemyAction;

public  class Enemy
{
    EnemyData enemyData;

    public int maxHealth;
    public int health;
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
}
