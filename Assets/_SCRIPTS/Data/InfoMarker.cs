using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class InfoMarker : ScriptableObject
    {
        public string Message;
        public GameTime Timestamp;
        public string DevNotes;
    }
}