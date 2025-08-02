using System;
using Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class InfoMarker
    {
        [SerializeField]
        public string Message;
        [SerializeField]
        public GameTime Timestamp;
        [SerializeField]
        public string DevNotes;
        [SerializeField]
        public Channel Feature;
        [SerializeField]
        public Person SenderPerson;
    }
}