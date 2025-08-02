using DefaultNamespace;
using TMPro;
using UnityEngine;

public class EmailListItemBehavior : MonoBehaviour
{

    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateView(EmailData emailData)
    {
        header.text = emailData.Header;
        body.text = emailData.Body;
    }
}
