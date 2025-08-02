using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Person")]
    public class ReceivedNewsData : ScriptableObject
    {
        [SerializeField] public string Header;
        [SerializeField] public string Body;
    }
}