using UnityEngine;
using System.Collections.Generic;

public abstract class Unit {
    public int maxHealth;
    public int health;

    public void damage(int damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            die();
        }
    }

    public abstract void die();
}