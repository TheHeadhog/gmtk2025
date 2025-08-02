using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class Person : ScriptableObject
    {
        public string FullName;
        public string Email;
        public string PhoneNumber;
    }
}
