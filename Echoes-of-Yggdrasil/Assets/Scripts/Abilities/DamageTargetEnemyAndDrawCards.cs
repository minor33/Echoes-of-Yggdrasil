using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DamageTargetEnemyAndDrawCards")]
public class DamageTargetEnemyAndDrawCards : Ability
{
    public int numCards;
    public int damage;
    
    public override string getDescription() {
        return $"Damage a target enemy by {damage} and draw {numCards} cards.";
    }
}