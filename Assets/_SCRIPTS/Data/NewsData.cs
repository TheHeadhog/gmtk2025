namespace DefaultNamespace
{
    public class NewsData
    {
        public string Header;
        public string Body;
        public int TabNumber;

        public NewsData(string header, string body, int tabNumber)
        {
            Header = header;
            Body = body;
            TabNumber = tabNumber;
        }
    }
}