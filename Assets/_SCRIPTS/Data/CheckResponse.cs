using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/CheckResponse")]
    public class CheckResponse : ScriptableObject
    {
        public string Message;
        public GameTime BeginTimestampInclusive;
        public GameTime EndTimestampExclusive;
        public int Points;
    }
}