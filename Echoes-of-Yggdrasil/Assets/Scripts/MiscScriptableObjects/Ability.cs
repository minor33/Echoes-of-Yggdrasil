using UnityEngine;
using System;
using System.Collections.Generic;
using static GameConstants;
using static Keyword;


[Serializable]
public struct KeywordPair
{
    public Keyword keyword;
    [Tooltip("For Target: 0=Choose,1=Random,2=Front,3=Self")]
    public int value;
}

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject {
    public List<KeywordPair> keywords;

    public string getDescription() {
        if (keywords == null) {
            return "Does Nothing";
        }
        string description = "";
        int target = -1;
        foreach (var keywordPair in keywords) {
            Keyword keyword = keywordPair.keyword;
            int input = keywordPair.value;
            
            switch(keyword) {
                case TARGET:
                    target = input;
                    break;

                case DAMAGE:
                    if (target == CHOOSE) {
                        description += $"Deal {input} damage. ";
                    } else if (target == RANDOM) {
                        description += $"Deal {input} damage to a random enemy. ";
                    } else if (target == FRONT) {
                        description += $"Deal {input} damage to the front enemy. ";
                    } else if (target == SELF) {
                        description += $"Take {input} self damage. ";
                    } else {
                        description += $"ERROR: Damage with {target} not defined";
                    }
                    break;

                case BLOCK:
                    description += $"Defend {input}. ";
                    break;
                
                case DRAW:
                    description += $"Draw {input}. ";
                    break;

                default:
                    description += $"ERROR: {keyword} not defined";
                    break;
            }
        }
        return description;
    }
}
