using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DamageRandomEnemy")]
public class DamageRandomEnemy : Ability
{
    public int damage;
    
    public override string getDescription() {
        return $"Damage a random enemy by {damage}.";
    }
}