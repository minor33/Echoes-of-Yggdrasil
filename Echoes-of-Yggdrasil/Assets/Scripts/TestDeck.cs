using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TestDeck")]
public class TestDeck : ScriptableObject
{
    public List<CardData> cards;
}
