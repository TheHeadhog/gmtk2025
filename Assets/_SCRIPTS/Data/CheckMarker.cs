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
        public Person SenderPerson;

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
        C1,
        C2,
        C3,
        C4,
        C5,
        C6,
        C7,
        C8,
        C9,
        C10
    }

    public class SetMarker
    {
        public CheckMarkerId Id;
        public GameTime Timestamp;
    }
}