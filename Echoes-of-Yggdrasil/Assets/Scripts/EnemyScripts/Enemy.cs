using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public int maxHealth;
    public int health;
    
    //public List<Action> actionList;
    public int currentAction;

    public enum Action {
        Attack,
        Block,
        LargeAttack,
        Pause
    }

    
}
