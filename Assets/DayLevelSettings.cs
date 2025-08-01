using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class DayLevelSettings : ScriptableObject
{
    public List<InfoMarker> InfoMarkers;
}
