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

    [ShowField(nameof(keyword), INVOKE)]
    public int invoke;

    [ShowField(nameof(keyword), DUPLICATE)]
    public int duplicate;
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

    public bool hasKeyword(Keyword keyword) {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == keyword) {
                return true;
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
                    description += $"Block {block}. ";
                    break;
                
                case DRAW:
                    int draw = keywordPair.draw;
                    description += $"Draw {draw}. ";
                    break;
                
                case INVOKE:
                    int invoke = keywordPair.invoke;
                    description += $"Invoke {invoke}. ";
                    break;

                case DUPLICATE:
                    int duplicate = keywordPair.duplicate;
                    description += $"Duplicate {duplicate}. ";
                    break;

                case STABLE:
                    description += $"Stable.";
                    break;

                default:
                    description += $"ERROR: {nameof(keyword)} not defined";
                    break;
            }
        }
        return description;
    }

    public void trigger(Enemy chosenEnemy=null) {
        battleManager = BattleManager.Instance;
        boardManager = BoardManager.Instance;
        player = BattlePlayer.Instance;
        Unit unit = boardManager.getFrontEnemy(); // Should this just be null?  -  No, default is front enemy to not crash everything
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
                    player.gainBlock(keywordPair.block);
                    break;
                
                case DRAW:
                    player.drawCards(keywordPair.draw);
                    break;

                case INVOKE:
                    player.addInvoke(keywordPair.invoke);
                    break;

                case DUPLICATE:
                    player.addDuplicate(keywordPair.duplicate);
                    break;

                // To be filled in with keywords which have no effect on play/trigger
                case STABLE:
                    if (DEBUG) {
                        Debug.Log($"{nameof(keyword)} intentionally has no function in trigger");
                    }
                    break;

                default:
                    Debug.LogError($"{nameof(keyword)} NOT DEFINED IN TRIGGER FUNCTION");
                    break;
            }
        }
    }
}
