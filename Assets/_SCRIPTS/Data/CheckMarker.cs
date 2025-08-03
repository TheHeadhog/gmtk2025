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
        public string GoodResponse;

        private const string DefaultResponse = "Why did you screw up my meeting???";
        
        public CheckResponse GetBadResponse(int gameTicks)
        {
            var response = Responses.FirstOrDefault(r =>
                gameTicks >= r.BeginTimestampInclusive.ToGameTick() && gameTicks < r.EndTimestampExclusive.ToGameTick());
            return response ?? new CheckResponse(DefaultResponse, 0);
        }

        public CheckResponse GetGoodResponse()
        {
            return new CheckResponse(GoodResponse, Points);
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