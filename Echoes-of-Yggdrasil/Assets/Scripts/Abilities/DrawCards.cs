using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DrawCards")]
public class DrawCards : Ability
{
    public int numCards;
    
    public override string getDescription() {
        return $"Draw {numCards} cards.";
    }
}