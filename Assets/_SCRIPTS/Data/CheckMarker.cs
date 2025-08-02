using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheckMarker : ScriptableObject
    {
        public GameTime Timestamp;
        public List<CheckResponse> Responses;
        public int Points;
        public int Id;
    }
}