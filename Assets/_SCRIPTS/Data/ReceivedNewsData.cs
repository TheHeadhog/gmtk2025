using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct ReceivedNewsData
    {
        public string Body;
        public string Header => GetFirstTwoWords(Body);

        public ReceivedNewsData(string message)
        {
            Body = message;
        }
        
        private string GetFirstTwoWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var parts = input.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return parts.Length >= 2 ? $"{parts[0]} {parts[1]}" : parts[0];
        }
    }
}