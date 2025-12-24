using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Event")]
public class Event : ScriptableObject  
{
    public Color color;

    public string text;
    public List<string> options;
}
