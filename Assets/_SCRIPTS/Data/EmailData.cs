namespace DefaultNamespace
{
    public class EmailData
    {
        public string Header;
        public string Body;

        public EmailData(string header, string body)
        {
            this.Header = header;
            this.Body = body;
        }
    };
}