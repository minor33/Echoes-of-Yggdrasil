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
    /*
    public string getPlayAbilityDescription() {
        return cardData.playAbility.getDescription();
    }
    public string getRageAbilityDescription() {
        return cardData.rageAbility.getDescription();
    }
    */
}
