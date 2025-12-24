using UnityEngine;

public class Card
{
    public CardData cardData;

    public Card(CardData cardData) {
        this.cardData = cardData;
    }

    public void play(Enemy targetEnemy = null) {
        BattlePlayer.Instance.energy -= this.getEnergy();
        BattlePlayer.Instance.rage += this.getRage();
        if (this.getRage() == 99) { // 99 Represents instant trigger
            BattlePlayer.Instance.rage = BattlePlayer.Instance.getMaxRageQueue();
        }
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
