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

    // I really wish there was a way to generalize these getters, I should look into it because there might
    public int getRetain() {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == RETAIN) {
                return keywordPair.retain;
            }
        }
        // Keyword RETAIN does not exist
        return 0;
    }

    public int getStarter() {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == STARTER) {
                return keywordPair.starter;
            }
        }
        // Keyword STARTER does not exist
        return 0;
    }

    public int getFinisher() {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == FINISHER) {
                return keywordPair.finisher;
            }
        }
        // Keyword FINISHER does not exist
        return 0;
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
