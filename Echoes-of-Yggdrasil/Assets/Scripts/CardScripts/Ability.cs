using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
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

    [ShowField(nameof(keyword), ANGRIER)]
    public int angrier;

    [ShowField(nameof(keyword), CALM_DOWN)]
    public int calm_down;

    [ShowField(nameof(keyword), SKIP)]
    public int skip;

    [ShowField(nameof(keyword), RETAIN)]
    public int retain;

    [ShowField(nameof(keyword), SWAP)]
    public int swap;

    [ShowField(nameof(keyword), STARTER)]
    public int starter;

    [ShowField(nameof(keyword), FINISHER)]
    public int finisher;

    [ShowField(nameof(keyword), EXPAND)]
    public int expand;

    [ShowField(nameof(keyword), REMOVE)]
    public int remove;

    [ShowField(nameof(keyword), REPEAT)]
    public int repeat;
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

    // Handles all int keywords (everything but TARGET)
    public int getKeywordValue(Keyword keyword) {
        foreach (var keywordPair in keywords) {
            if (keywordPair.keyword == keyword) {
                return getValueByKeyword(keywordPair, keyword);
            }
        }
        return 0;
    }

    // Every keyword pair SHOULD be added to this list. 
    // But technically it only needs to be added if it needs to be accessed outside of this file.
    private int getValueByKeyword(KeywordPair pair, Keyword keyword) {
        switch (keyword) {
            case RETAIN: return pair.retain;
            case STARTER: return pair.starter;
            case FINISHER: return pair.finisher;
            case DAMAGE: return pair.damage;
            case BLOCK: return pair.block;
            case DRAW: return pair.draw;
            case INVOKE: return pair.invoke;
            case DUPLICATE: return pair.duplicate;
            case ANGRIER: return pair.angrier;
            case CALM_DOWN: return pair.calm_down;
            case SKIP: return pair.skip;
            case SWAP: return pair.swap;
            case EXPAND: return pair.expand;
            case REMOVE: return pair.remove;
            case REPEAT: return pair.repeat;
            default: return 0;
        }
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

                case ANGRIER:
                    int angrier = keywordPair.angrier;
                    description += $"Angrier {angrier}. ";
                    break;

                case CALM_DOWN:
                    int calm_down = keywordPair.calm_down;
                    description += $"Calm Down {calm_down}. ";
                    break;

                case SKIP:
                    int skip = keywordPair.skip;
                    description += $"Skip {skip}. ";
                    break;

                case RETAIN:
                    // So, the description doesn't update as the retain gets updated. That def needs to happen. 
                    // Or a different display that shows it
                    int retain = keywordPair.retain;
                    description += $"Retain {retain}. ";
                    break;

                case TACTICAL:
                    description += "Tactical.";
                    break;

                case SWAP: 
                    int swap = keywordPair.swap;
                    description += $"Swap {swap}.";
                    break;

                case STARTER:
                    int starter = keywordPair.starter;
                    description += $"Starter {starter}.";
                    break;

                case FINISHER:
                    int finisher = keywordPair.finisher;
                    description += $"Starter {finisher}.";
                    break;

                case EXPAND:
                    int expand = keywordPair.expand;
                    description += $"Expand {expand}.";
                    break;

                case REMOVE:
                    int remove = keywordPair.remove;
                    description += $"Remove {remove}.";
                    break;

                case REVERSE:
                    description += "Reverse the rage queue.";
                    break;

                case REPEAT:
                    int repeat = keywordPair.repeat;
                    description += $"Repeat {repeat}.";
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
                    int rageAdjustment = BattlePlayer.Instance.getRageAdjustment();
                    if (DEBUG) {
                        Debug.Log($"Doing {keywordPair.damage} damage to {unit} with a rage adjustment of {rageAdjustment} for {keywordPair.damage+rageAdjustment} total damage");
                    }
                    unit.damage(keywordPair.damage + rageAdjustment);
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
                
                case ANGRIER:
                    player.addRageAdjustment(keywordPair.angrier);
                    break;

                case CALM_DOWN:
                    player.addRageAdjustment(-keywordPair.calm_down);
                    break;

                case SKIP:
                    player.addSkip(keywordPair.skip);
                    break;

                case SWAP:
                    player.swapRageCardSelection(keywordPair.swap);
                    break;

                case EXPAND:
                    player.addExpand(keywordPair.expand);
                    break;

                case REMOVE:
                    player.removeRageCardSelection(keywordPair.remove);
                    break;

                case REVERSE:
                    player.reverseRageQueue();
                    break;

                // To be filled in with keywords which have no effect on play/trigger
                case STABLE:
                case RETAIN:
                case TACTICAL:
                case STARTER:
                case FINISHER:
                case REPEAT:
                    if (DEBUG) {
                        Debug.Log($"{keyword} intentionally has no function in trigger");
                    }
                    break;

                default:
                    Debug.LogError($"{keyword} NOT DEFINED IN TRIGGER FUNCTION");
                    break;
            }
        }
    }
}
