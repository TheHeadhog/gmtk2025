using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class EmailBehavior : MonoBehaviour
{
    private List<EmailData> emailList = new List<EmailData>()
    {
        new EmailData("greta hr","Disciplinary Chat (Confidential)","This is mail body 1"),
        new EmailData("bob","You Have Lactose Intolerance","This is mail body 2"),
        new EmailData("loopbot","Time Audit Alert","This is mail body 3"),
    };
    private int CurrentlySelectedEmailId = 0;
    [SerializeField] 
    private GameObject emailListGameObject;
    [Header("MailPreviewReferences")]
    [SerializeField]
    private TMP_Text Sender;
    [SerializeField]
    private TMP_Text Header;
    [SerializeField]
    private TMP_Text Body;

    [SerializeField] private GameObject EmailListItemPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.OnInfoMarkerAppear += OnInfoMarkerAppear;
        GameEvents.OnEmailClicked += ChangeEmailPreview;
        UpdateMailList();
        UpdatePreviewMail(emailList.Find((_ => true)));
    }

    private void ChangeEmailPreview(EmailData newPreviewMail)
    {
        UpdatePreviewMail(newPreviewMail);
    }

    private void OnInfoMarkerAppear(InfoMarker marker)
    {
        emailList.Add(new EmailData(marker));
        UpdateMailList();
    }

    private void UpdateMailList()
    {
        foreach (Transform item in emailListGameObject.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var emailData in emailList)
        {
            var newEmailListItem = Instantiate(EmailListItemPrefab,emailListGameObject.transform);
            newEmailListItem.GetComponent<EmailListItemBehavior>().UpdateView(emailData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePreviewMail(EmailData emailData)
    {
        if (emailData is null) return;
        Header.text = emailData.Header;
        Body.text = emailData.Body;
        Sender.text = emailData.Sender;
    }
}
