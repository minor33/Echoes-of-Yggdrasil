using UnityEngine;
using System.Collections.Generic;
using System;
using EditorAttributes;
using static EnemyAction;


[Serializable]
public struct ActionPair
{
    public EnemyAction action;
   
    [ShowField(nameof(action), ATTACK)]
    public int damage;

    [ShowField(nameof(action), DEFEND)]
    public int defend;
}

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public int maxHealth;
    //public Image image;
    public List<ActionPair> actionPairs; 
}
