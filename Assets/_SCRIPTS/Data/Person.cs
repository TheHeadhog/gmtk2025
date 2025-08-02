using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Person")]
    public class Person : ScriptableObject
    {
        public string FullName;
        public string Email;
        public string PhoneNumber;
    }
}
