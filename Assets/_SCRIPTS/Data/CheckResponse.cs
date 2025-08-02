using System;

namespace DefaultNamespace
{
    [Serializable]
    public class CheckResponse
    {
        public string Message;
        public GameTime BeginTimestampInclusive;
        public GameTime EndTimestampExclusive;
        public int Points;

        public CheckResponse(string message, int points)
        {
            Message = message;
            Points = points;
        }
    }
}