using UnityEngine;
using static EnemyAction;

public class Slime : Enemy
{
    public Slime() : base() {
        maxHealth = 10;
        health = maxHealth;
    }
}