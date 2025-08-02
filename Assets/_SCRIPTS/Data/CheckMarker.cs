using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class CheckMarker : ScriptableObject
    {
        public GameTime Timestamp;
        public List<CheckResponse> Responses;
        public int Points;
        public int Id;
    }
}