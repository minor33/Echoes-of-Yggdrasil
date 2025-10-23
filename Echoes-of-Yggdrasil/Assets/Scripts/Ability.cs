using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct KeywordPair
{
    public enum Keyword {
        Damage,
        Block,
        Target,
        Draw
    }
    public Keyword keyword;
    [Tooltip("For Target: 0=Choose,1=Random,2=Self")]
    public int value;
}

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject {
    public List<KeywordPair> keywords;

    
}
