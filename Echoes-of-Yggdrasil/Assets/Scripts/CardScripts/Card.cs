using UnityEngine;

public class Card
{
    public CardData cardData;

    public Card(CardData cardData) {
        this.cardData = cardData;
    }

    public void play(Enemy targetEnemy = null) {
        // Energy
        BattlePlayer.Instance.rage += this.getRage();
        Debug.Log(BattlePlayer.Instance.rage);
        getPlayAbility().trigger(targetEnemy);
    }

    public void triggerRage() {
        getRageAbility().trigger();
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
