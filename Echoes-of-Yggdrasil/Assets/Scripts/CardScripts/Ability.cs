using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using EditorAttributes;
using static GameConstants;
using static Keyword;
using static Target;
using static StatusKeyword;


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

    [ShowField(nameof(keyword), SET_EVOKE)]
    public Ability setEvoke;

    [ShowField(nameof(keyword), EVOKE)]
    public int evoke;

    [ShowField(nameof(keyword), RECALL)]
    public int recall;

    [ShowField(nameof(keyword), FOCUS)]
    public int focus;
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

    public Ability getEvokeSetter() {
        foreach (var keywordPair in keywords){
            if(keywordPair.keyword == SET_EVOKE) {
                return keywordPair.setEvoke;
            }
        }
        return null;
    }

    // Handles all int keywords (everything but TARGET and SET_EVOKE)
    public int getKeywordValue(Keyword keyword) {
        foreach (var keywordPair in keywords) {
            if (keywordPair.keyword == keyword) {
                return getValueOfKeywordPair(keywordPair);
            }
        }
        return 0;
    }

    // Every keyword pair NEEDS to be added to this list if it's an integer. 
    public int getValueOfKeywordPair(KeywordPair pair) {
        switch (pair.keyword) {
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
            case EVOKE: return pair.evoke;
            case RECALL: return pair.recall;
            case FOCUS: return pair.focus;
            default: return 0;
        }
    }
    
    // Description should probably be set when created, and then gotten from here, since getDescription is called in multiple places
    // This function should be optomized using the above get function
    public string getDescription() {
        if (keywords == null) {
            return "Does Nothing";
        }
        string description = "";
        string keywordText = "";
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

                // Case for when description is "{KEYWORD} {VALUE}."
                // Requires keyword to be named the same as you want the description
                case BLOCK: // Block likely will need to be seperated from this later
                case DRAW:
                case INVOKE:
                case DUPLICATE:
                case ANGRIER:
                case CALM_DOWN:
                case SKIP:
                case RETAIN:
                case SWAP: 
                case STARTER:
                case FINISHER:
                case EXPAND:
                case REMOVE:
                case REPEAT:
                case EVOKE:
                case RECALL:
                case FOCUS:
                    keywordText = $"{keyword}";
                    keywordText = char.ToUpper(keywordText[0]) + keywordText.Substring(1).ToLower();
                    description += keywordText + $" {getValueOfKeywordPair(keywordPair)}. ";
                    break;
                
                // Case for when description is "{KEYWORD}."
                // Requires keyword to be named the same as you want the description
                case STABLE:
                case TACTICAL:
                case PATIENT:
                case EXHAUST:
                    keywordText = $"{keyword}";
                    keywordText = char.ToUpper(keywordText[0]) + keywordText.Substring(1).ToLower();
                    description += keywordText + ". ";
                    break;

                // All other cases
                case REVERSE:
                    description += "Reverse the rage queue. ";
                    break;

                case SET_EVOKE:
                    Ability ability = keywordPair.setEvoke;
                    description += $"Set Evoke: {ability.getDescription()} ";
                    break;

                default:
                    description += $"ERROR: {keyword} not defined";
                    break;
            }
        }
        // Perform no no checks for easy to forget interactions here:
        if (hasKeyword(SET_EVOKE) && getEvokeSetter().hasKeyword(EVOKE)) {
            description += "ERROR: Card sets evoke to an evoke ability. This will loop forever.";
        }
        if (hasKeyword(SET_EVOKE) && getEvokeSetter().hasChoose()) {
            description += "ERROR: Card sets evoke to an ability with a choose effect. This may cause crashes and unexpected behavior.";
        }

        return description;
    }

    // numTriggers allows an ability to be triggered multiple times. However, a lot of times, it's handled elsewhere for animations,
    // such as in the rageQueue. It doesn't have to be used but can be helpful. 
    public void trigger(Enemy chosenEnemy=null, int numTriggers=1, bool first=true, bool fromRecall=false) {
        if (numTriggers <= 0) {
            if (DEBUG) {
                Debug.Log($"Ending ability trigger with description {getDescription()}");
            }
            return;
        } else if (DEBUG && first) {
            Debug.Log($"Triggering ability with following description {numTriggers} times: {getDescription()}");
        }

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
            Ability ability;
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

                case SET_EVOKE:
                    player.setEvokeAbility(keywordPair.setEvoke);
                    break;

                case EVOKE:
                    int evoke = keywordPair.evoke;
                    ability = player.getEvokeAbility();
                    if (ability == null) {
                        if (DEBUG) {
                            Debug.Log("No ability to evoke");
                        }
                        break;
                    }
                    if (DEBUG) {
                        Debug.Log($"Triggering evoke ability with following description {evoke} times: {ability.getDescription()}");
                    }
                    ability.trigger(chosenEnemy, evoke);
                    break;

                case RECALL:
                    int recall = keywordPair.recall;
                    if (player.discard.Count <= 0) {
                        if (DEBUG) {
                            Debug.Log("Nothing to Recall, doing nothing");
                        }
                        break;
                    }
                    if (fromRecall) {
                        if (DEBUG) {
                            Debug.Log("Recall trigger is from a recall, doing nothing");
                        }
                        break;
                    }
                    ability = player.getTopDiscard().getRageAbility();
                    if (DEBUG) {
                        Debug.Log($"Triggering recall ability with following description {recall} times: {ability.getDescription()}");
                    }
                    ability.trigger(chosenEnemy, recall, fromRecall: true);
                    break;

                case FOCUS:
                    int focus = keywordPair.focus;
                    player.addStatusEffect(SFOCUS, focus);
                    Debug.Log(player.getStatusEffect(SFOCUS));
                    break;

                // To be filled in with keywords which have no effect on play/trigger
                case STABLE:
                case RETAIN:
                case TACTICAL:
                case STARTER:
                case FINISHER:
                case REPEAT:
                case PATIENT:
                case EXHAUST:
                    if (DEBUG) {
                        Debug.Log($"{keyword} intentionally has no function in trigger");
                    }
                    break;

                default:
                    Debug.LogError($"{keyword} NOT DEFINED IN TRIGGER FUNCTION");
                    break;
            }
        }
        // Recursive looping to prevent too many tabs. Shouldn't ruin anything. 
        trigger(chosenEnemy, --numTriggers, false, fromRecall);
    }

}
