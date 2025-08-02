using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class CheckResponse : ScriptableObject
    {
        public string Message;
        public GameTime BeginTimestampInclusive;
        public GameTime EndTimestampExclusive;
    }
}