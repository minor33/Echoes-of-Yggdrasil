using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public struct Choice {
    public string text;
    public string description;
}


[CreateAssetMenu(menuName = "Event")]
public class Event : ScriptableObject  
{
    public Color color;

    public string text;
    public List<Choice> choices;
}
