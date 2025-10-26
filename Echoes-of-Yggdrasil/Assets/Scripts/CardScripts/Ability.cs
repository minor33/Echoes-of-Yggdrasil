using UnityEngine;
using System;
using System.Collections.Generic;
using EditorAttributes;
using static GameConstants;
using static Keyword;
using static Target;


[Serializable]
public struct KeywordPair
{
    public Keyword keyword;

    [ShowField(nameof(keyword), DAMAGE)]
    public int damage;

    [ShowField(nameof(keyword), BLOCK)]
    public int block;

    [ShowField(nameof(keyword), DRAW)]
    public int draw;

    [ShowField(nameof(keyword), TARGET)]
    public Target target;
}

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject {
    public List<KeywordPair> keywords;
    private BattleManager battleManager;
    private BoardDisplay boardDisplay;

    public bool hasChoose() {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == TARGET){
                if(keywordPair.target == CHOOSE){
                    return true;
                }
            }
        }
        return false;
    }

    public string getDescription() {
        if (keywords == null) {
            return "Does Nothing";
        }
        string description = "";
        Target target = FRONT;
        foreach (var keywordPair in keywords) {
            Keyword keyword = keywordPair.keyword;
            
            switch(keyword) {
                case TARGET:
                    target = keywordPair.target;
                    break;

                case DAMAGE:
                    int damage = keywordPair.damage;
                    if (target == CHOOSE) {
                        description += $"Deal {damage} damage. ";
                    } else if (target == RANDOM) {
                        description += $"Deal {damage} damage to a random enemy. ";
                    } else if (target == FRONT) {
                        description += $"Deal {damage} damage to the front enemy. ";
                    } else if (target == SELF) {
                        description += $"Take {damage} self damage. ";
                    } else {
                        description += $"ERROR: Damage with {target} not defined";
                    }
                    break;

                case BLOCK:
                    int block = keywordPair.block;
                    description += $"Defend {block}. ";
                    break;
                
                case DRAW:
                    int draw = keywordPair.draw;
                    description += $"Draw {draw}. ";
                    break;

                default:
                    description += $"ERROR: {keyword} not defined";
                    break;
            }
        }
        return description;
    }

    public void triggerAbility(Enemy chosenEnemy=null) {
        battleManager = BattleManager.Instance;
        boardDisplay = BoardDisplay.Instance;
        Unit unit = boardDisplay.getEnemy();
        if (unit == null) {
            Debug.LogError("NO ENEMY FOUND AFTER TRIGGERING ABILITY");
            return;
        }

        foreach (var keywordPair in keywords) {
            Keyword keyword = keywordPair.keyword;
            
            switch(keyword) {
                case TARGET:
                    Target targetType = keywordPair.target;
                    if (targetType == CHOOSE) {
                        unit = chosenEnemy;
                    } else if (targetType == RANDOM) {
                        // TODO: lol make this random later
                        Debug.LogError("THIS TARGETTING DOES NOT EXIST YET");
                        unit = boardDisplay.getEnemy();
                    } else if (targetType == FRONT) {
                        unit = boardDisplay.getEnemy();
                    } else if (targetType == SELF) {
                        unit = BattlePlayer.Instance;
                    }
                    break;

                case DAMAGE:
                    int damage = keywordPair.damage;
                    unit.damage(damage);
                    break;

                case BLOCK:
                    int block = keywordPair.block;
                    Debug.LogError("DOES NOT EXIST YET");
                    break;
                
                case DRAW:
                    int draw = keywordPair.draw;
                    Debug.LogError("DOES NOT EXIST YET");
                    break;

                default:
                    break;
            }
        }
    }
}
