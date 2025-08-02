using DefaultNamespace;
using TMPro;
using UnityEngine;

public class EmailListItemBehavior : MonoBehaviour
{
    private EmailData emailData;
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text sender;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateView(EmailData emailData)
    {
        this.emailData = emailData;
        header.text = emailData.Header;
        sender.text = emailData.Sender;
    }

    public void OnClick()
    {
        Debug.Log("CLICKED");
        GameEvents.RaiseEmailClicked(emailData);
    }
}
