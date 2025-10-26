using UnityEngine;

public class Card
{
    public CardData cardData;

    public Card(CardData cardData) {
        this.cardData = cardData;
    }

    public int getEnergy() {
        return cardData.energy;
    }
    public int getRage() {
        return cardData.rage;
    }
    public string getName() {
        return cardData.name;
    }

    public Ability getPlayAbility() {
        return cardData.playAbility;
    }

    public Ability getRageAbility() {
        return cardData.rageAbility;
    }
    
    public string getPlayAbilityDescription() {
        return getPlayAbility().getDescription();
    }
    public string getRageAbilityDescription() {
        return getRageAbility().getDescription();
    }
    
}
