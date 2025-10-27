using UnityEngine;
using System.Collections.Generic;
using System;
using EditorAttributes;
using UnityEngine.UI;
using static EnemyAction;


[Serializable]
public struct ActionPair
{
    public EnemyAction action;
   
    [ShowField(nameof(action), ATTACK)]
    public int attack;

    [ShowField(nameof(action), DEFEND)]
    public int defend;
}

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public int maxHealth;
    public Sprite sprite;
    public List<ActionPair> actionPairs; 
}
