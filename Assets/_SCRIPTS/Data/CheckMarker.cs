using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "CheckMarker", menuName = "Scriptable Objects/CheckMarker")]
    public class CheckMarker : ScriptableObject
    {
        public CheckMarkerId Id;
        public GameTime Timestamp;
        public int DurationInMinutes;
        public List<CheckResponse> Responses;
        public int Points;
        public string Message;
        public Channel Feature;
        public string TextOnCalendarTile;

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
        Test1,
        Test2,
        Test3
    }

    public class SetMarker
    {
        public CheckMarkerId Id;
        public GameTime Timestamp;
    }
}