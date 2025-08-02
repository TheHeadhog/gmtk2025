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
    };
}