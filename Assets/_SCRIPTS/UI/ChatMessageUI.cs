using TMPro;
using UnityEngine;

public class ChatMessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _body;

    public void Init(string txt)
    {
        _body.text = txt;
    }
}