using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Person", menuName = "Scriptable Objects/Person")]
    public class Person : ScriptableObject
    {
        public string FullName;
        public string Email;
        public string PhoneNumber;
        public Sprite Avatar;
    }
}
