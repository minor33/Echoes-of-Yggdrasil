using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu( menuName = "World")]
public class World : ScriptableObject
{
    public int worldTier;

    public List<Encounter> encounters;
    public List<Event> events;
    public List<CardData> worldCards;

    public Encounter bossEncounter;

    public Color color;
}
