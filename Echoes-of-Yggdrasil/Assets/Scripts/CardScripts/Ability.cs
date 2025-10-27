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
    private BoardManager boardManager;
    private BattlePlayer player;

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

    public void trigger(Enemy chosenEnemy=null) {
        battleManager = BattleManager.Instance;
        boardManager = BoardManager.Instance;
        player = BattlePlayer.Instance;
        Unit unit = boardManager.getFrontEnemy();
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
                        unit = boardManager.getRandomEnemy();
                    } else if (targetType == FRONT) {
                        unit = boardManager.getFrontEnemy();
                    } else if (targetType == SELF) {
                        unit = player;
                    }
                    break;

                case DAMAGE:
                    unit.damage(keywordPair.damage);
                    break;

                case BLOCK:
                    Debug.LogError("DOES NOT EXIST YET");
                    break;
                
                case DRAW:
                    for(int i = 0; i < keywordPair.draw; i++){
                        player.drawCard();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
