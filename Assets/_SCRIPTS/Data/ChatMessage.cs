using DefaultNamespace;

public struct ChatMessage
{
    public readonly Person Sender;
    public readonly string Text;

    public ChatMessage(Person sender, string text)
    {
        Sender = sender;
        Text = text;
    }
}