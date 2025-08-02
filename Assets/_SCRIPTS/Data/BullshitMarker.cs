using Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/BullshitMarker")]
    public class BullshitMarker : ScriptableObject
    {
        public string Message;
        public GameTime Timestamp;
        public Channel Feature;
        public Person SenderPerson;
        public ReceivedNewsData ReceivedNewsData;
        public EmailData EmailData;
    }
}