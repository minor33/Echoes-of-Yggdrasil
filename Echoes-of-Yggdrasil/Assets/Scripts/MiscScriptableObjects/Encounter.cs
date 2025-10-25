using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Encounter")]
public class Encounter : ScriptableObject
{
    [Tooltip("Should always be size 3 no matter what")]
    public EnemyData[] enemies;

    //public Sprite background;
}
