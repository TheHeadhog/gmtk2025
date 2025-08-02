using UnityEngine;

namespace DefaultNamespace
{
    public class CheckResponse : ScriptableObject
    {
        public string Message;
        public GameTime BeginTimestampInclusive;
        public GameTime EndTimestampExclusive;
        public int Points;
    }
}