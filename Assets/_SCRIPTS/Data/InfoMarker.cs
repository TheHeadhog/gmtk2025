using Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/InfoMarker")]
    public class InfoMarker : ScriptableObject
    {
        public string Message;
        public GameTime Timestamp;
        public string DevNotes;
        public Channel Feature;
        public Person SenderPerson;
    }
}