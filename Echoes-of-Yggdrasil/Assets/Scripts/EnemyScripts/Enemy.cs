using UnityEngine;
using System.Collections.Generic;

public enum Action {
        Attack,
        Block,
        LargeAttack,
        Pause
}

[CreateAssetMenu(menuName = "Enemy")]
public abstract class Enemy : ScriptableObject
{
    public int maxHealth = 1;
    public int health = 1;
    public int blockAmount = 1;
    public int attackAmount = 1;
    public int largeAttackAmount = 2;

    //public Image image;
    
    //public List<Action> actionOrder = new List<Action> {Action.Attack, Action.Block, Action.LargeAttack, Action.Pause};
    //public int currentAction = Random.Range(0, actionOrder.Count);
}
