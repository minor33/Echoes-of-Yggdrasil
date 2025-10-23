using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class CardData : ScriptableObject
{
    public CardType cardType;
    public CardRarity cardRarity;
    public int energy;
    public int rage;
    //public Image image;
    //public God god;
    public Ability playAbility;
    public Ability rageAbility;

    public enum CardType {
        Attack,
        Skill,
        Power
    }
    public enum CardRarity {
        Starter,
        Common,
        Rare,
        Legendary
    }
}
