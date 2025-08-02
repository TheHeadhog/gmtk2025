using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/CheckMarker")]
    public class CheckMarker : ScriptableObject
    {
        public GameTime Timestamp;
        public List<CheckResponse> Responses;
        public int Points;
        public int Id;
    }
}