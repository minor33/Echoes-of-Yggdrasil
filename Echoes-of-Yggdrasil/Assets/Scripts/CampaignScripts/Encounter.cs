using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Encounter")]
public class Encounter : ScriptableObject
{
    public EnemyData[] enemies;
    //public Sprite background;
}
