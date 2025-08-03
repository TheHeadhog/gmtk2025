using System.Collections.Generic;
using DefaultNamespace;
using Helpers;

public class ChatManager : SingletonPersistent<ChatManager>
{
    public delegate void MessageArrived(ChatMessage msg);

    public static event MessageArrived OnMessageArrived;

    private readonly List<ChatMessage> _history = new();

    private void OnEnable()
    {
        GameEvents.OnBullshitMarkerAppear += HandleBullshit;
        GameEvents.OnInfoMarkerAppear += HandleInfo;
    }

    private void OnDisable()
    {
        GameEvents.OnBullshitMarkerAppear -= HandleBullshit;
        GameEvents.OnInfoMarkerAppear -= HandleInfo;
    }

    private void HandleBullshit(BullshitMarker marker)
    {
        TryAdd(marker.Feature, marker.SenderPerson, marker.Message);
    }

    private void HandleInfo(InfoMarker marker)
    {
        TryAdd(marker.Feature, marker.SenderPerson, marker.Message);
    }

    private void TryAdd(Channel ch, Person p, string txt)
    {
        if (ch != Channel.CHAT || p == null || string.IsNullOrWhiteSpace(txt)) return;
        
        var cm = new ChatMessage(p, txt);
        _history.Add(cm);
        OnMessageArrived?.Invoke(cm);
    }

    public IReadOnlyList<ChatMessage> GetHistory(Person person)
    {
        return _history.FindAll(m => m.Sender == person);
    }
}