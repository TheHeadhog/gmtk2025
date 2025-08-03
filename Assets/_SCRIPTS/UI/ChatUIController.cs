using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIController : MonoBehaviour
{
    [Header("Refs")] [SerializeField] private TMP_Text _headerLabel;
    [SerializeField] private Transform _sidebarParent;
    [SerializeField] private PersonaButtonUI _buttonPrefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private ChatMessageUI _bubblePrefab;

    [Header("Personas (order = button order)")] [SerializeField]
    private List<Person> _persons;

    private readonly Dictionary<Person, PersonaButtonUI> _buttons = new();
    private readonly List<ChatMessageUI> _spawned = new();
    private Person _current;

    private void Awake()
    {
        BuildSidebar();
    }

    private void OnEnable()
    {
        ChatManager.OnMessageArrived += OnMsg;
    }

    private void OnDisable()
    {
        ChatManager.OnMessageArrived -= OnMsg;
    }

    private void BuildSidebar()
    {
        foreach (var p in _persons)
        {
            var b = Instantiate(_buttonPrefab, _sidebarParent);
            b.Init(p, SelectPersona);
            _buttons.Add(p, b);
        }

        SelectPersona(_persons[0]);
    }

    private void SelectPersona(Person p)
    {
        _current = p;
        _headerLabel.text = p.FullName;
        foreach (var kv in _buttons)
        {
            kv.Value.SetSelected(kv.Key == p);
        }

        ClearChat();
        foreach (var m in ChatManager.Instance.GetHistory(p))
        {
            AddBubble(m.Text);
        }
    }

    private void OnMsg(ChatMessage msg)
    {
        if (msg.Sender == _current)
        {
            AddBubble(msg.Text);
        }
        else
        {
            if (_buttons.ContainsKey(msg.Sender))
            {
                _buttons[msg.Sender].ShowUnread();
            }
        }
    }

    private void AddBubble(string txt)
    {
        var ui = Instantiate(_bubblePrefab, _content);
        ui.Init(txt);
        _spawned.Add(ui);
        TrimOverflow();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
    }

    private void TrimOverflow()
    {
        while (_content.rect.height > _viewport.rect.height && _spawned.Count > 0)
        {
            Destroy(_spawned[0].gameObject);
            _spawned.RemoveAt(0);
        }
    }

    private void ClearChat()
    {
        foreach (var b in _spawned) Destroy(b.gameObject);
        _spawned.Clear();
    }
}