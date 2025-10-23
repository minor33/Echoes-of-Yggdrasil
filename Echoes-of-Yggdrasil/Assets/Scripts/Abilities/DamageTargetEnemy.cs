using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DamageTargetEnemy")]
public class DamageTargetEnemy : Ability
{
    public int damage;
    
    public override string getDescription() {
        return $"Damage a target enemy by {damage}.";
    }
}