using System;
using UnityEditor;

namespace DefaultNamespace
{
    public class EmailData
    {
        public string Sender;
        public string Header;
        public string Body;

        public EmailData(string sender,string header, string body)
        {
            this.Header = header;
            this.Body = body;
            this.Sender = sender;
        }

        public EmailData(InfoMarker infoMarker)
        {
            Header = $"Header {GUID.Generate()}";
            Body = infoMarker.Message;
            Sender = "Sender"; //todo stevanp: get real person data here
        }

        public EmailData(BullshitMarker bullshitMarker)
        {
            Header = $"Header {GUID.Generate()}";
            Body = bullshitMarker.Message;
            Sender = "Sender"; //todo stevanp: get real person data here as well
        }
    };
}