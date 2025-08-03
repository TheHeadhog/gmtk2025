#nullable enable
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using UnityEngine;

public class ChatManager : SingletonPersistent<ChatManager>
{
    public delegate void MessageArrived(ChatMessage msg);

    public static event MessageArrived OnMessageArrived;

    private readonly List<ChatMessage> _history = new();

    private void OnEnable()
    {
        GameEvents.OnBullshitMarkerAppear += HandleBullshit;
        GameEvents.OnInfoMarkerAppear += HandleInfo;
        GameEvents.OnGoodResponse += HandleCheckResponse;
        GameEvents.OnBadResponse += HandleCheckResponse;
    }

    private void OnDisable()
    {
        GameEvents.OnBullshitMarkerAppear -= HandleBullshit;
        GameEvents.OnInfoMarkerAppear -= HandleInfo;
        GameEvents.OnGoodResponse -= HandleCheckResponse;
        GameEvents.OnBadResponse -= HandleCheckResponse;
    }

    private void HandleBullshit(BullshitMarker marker)
    {
        TryAdd(marker.Feature, marker.SenderPerson, marker.Message);
    }

    private void HandleInfo(InfoMarker marker)
    {
        TryAdd(marker.Feature, marker.SenderPerson, marker.Message);
    }
    
    private void HandleCheckResponse(CheckResponse response, Person? senderPerson)
    {
        SendMessageToChat(senderPerson, response.Message);
    }

    private void SendMessageToChat(Person? sender,string message)
    {
        if (sender is null)
        {
            var systemErrorPerson = ScriptableObject.CreateInstance<Person>();
            systemErrorPerson.FullName = "S#st3M 3Rr0R";
            systemErrorPerson.Email="!@#.com";
            systemErrorPerson.Avatar = null;
            systemErrorPerson.PhoneNumber="#00000";
            sender = systemErrorPerson;
        }
        var chatMessage = new ChatMessage(sender, message);
        _history.Add(chatMessage);
        OnMessageArrived?.Invoke(chatMessage);
    }

    private void TryAdd(Channel ch, Person p, string txt)
    {
        if (ch != Channel.CHAT || p == null || string.IsNullOrWhiteSpace(txt)) return;
        
        SendMessageToChat(p,txt);
    }

    public IReadOnlyList<ChatMessage> GetHistory(Person person)
    {
        return _history.FindAll(m => m.Sender == person);
    }
}