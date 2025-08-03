using System;
using UnityEditor;

namespace DefaultNamespace
{
    public struct EmailData
    {
        public string SenderName;
        public string SenderEmail;
        public string Body;

        public string Header => GetFirstTwoWords(Body);

        public EmailData(string senderName,string senderEmail, string body)
        {
            this.SenderEmail = senderEmail;
            this.Body = body;
            this.SenderName = senderName;
        }

        public EmailData(InfoMarker infoMarker)
        {
            SenderEmail = infoMarker.SenderPerson.Email;
            Body = infoMarker.Message;
            SenderName = infoMarker.SenderPerson.FullName;
        }

        public EmailData(BullshitMarker bullshitMarker)
        {
            SenderEmail = bullshitMarker.SenderPerson.Email;
            Body = bullshitMarker.Message;
            SenderName = bullshitMarker.SenderPerson.FullName;
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
    };
}