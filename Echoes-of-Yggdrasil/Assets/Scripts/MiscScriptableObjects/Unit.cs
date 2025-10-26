using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour {
    protected int maxHealth;
    protected int health;

    public void damage(int damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            die();
        }
    }

    public abstract void die();
}