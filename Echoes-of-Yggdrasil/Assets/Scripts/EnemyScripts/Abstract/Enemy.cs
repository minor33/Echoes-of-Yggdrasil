using UnityEngine;
using System.Collections.Generic;
using static EnemyAction;

public abstract class Enemy
{
    public int maxHealth;
    public int health;

    //public Image image;
    
    public List<EnemyAction> actionOrder;
    public int currentAction;

    public Enemy () {
        actionOrder = new List<EnemyAction> {ATTACK, BLOCK, LARGE_ATTACK, PAUSE};
        currentAction = Random.Range(0, actionOrder.Count);
    }
}
