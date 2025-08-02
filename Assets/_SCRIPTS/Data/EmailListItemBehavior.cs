using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailListItemBehavior : MonoBehaviour
{
    private EmailData emailData;
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text sender;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GetComponent<Image>().color = transform.GetSiblingIndex() % 2 == 0 ? EmailGreenColor : EmailYellowColor;
    }

    [SerializeField] private Color EmailYellowColor;

    [SerializeField] private Color EmailGreenColor;

    public void UpdateView(EmailData emailData)
    {
        this.emailData = emailData;
        header.text = emailData.Header;
        sender.text = emailData.Sender;
    }

    public void OnClick()
    {
        GameEvents.RaiseEmailClicked(emailData);
    }
}
