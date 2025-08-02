using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/CheckMarker")]
    public class CheckMarker : ScriptableObject
    {
        public CheckMarkerId Id;
        public GameTime Timestamp;
        public List<CheckResponse> Responses;
        public int Points;
        public string Message;

        public CheckResponse GetBadResponse(int gameTicks)
        {
            return Responses.FirstOrDefault(r =>
                gameTicks >= r.BeginTimestampInclusive.ToGameTick() && gameTicks < r.EndTimestampExclusive.ToGameTick());
        }

        public CheckResponse GetGoodResponse()
        {
            return new CheckResponse(Message, Points);
        }
    }

    public enum CheckMarkerId
    {
        
    }

    public class SetMarker
    {
        public CheckMarkerId Id;
        public GameTime Timestamp;
    }
}