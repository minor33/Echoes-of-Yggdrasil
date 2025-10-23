using UnityEngine;
using System.Collections.Generic;
using System;
using static EnemyAction;


[Serializable]
public struct ActionPair
{
    public EnemyAction action;
    public int value;
    public int value2;
}

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public int maxHealth;
    //public Image image;
    public List<ActionPair> actionPairs; 
}
