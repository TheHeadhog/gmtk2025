using System;
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using TMPro;
using UnityEngine;

public class EmailBehavior : MonoBehaviour
{
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
    
    private List<EmailData> emailList = new();
    
    void Start()
    {
        GameEvents.OnInfoMarkerAppear += OnInfoMarkerAppear;
        GameEvents.OnEmailClicked += ChangeEmailPreview;
        GameEvents.OnBullshitMarkerAppear += OnBullshitMarkerAppear;
        UpdateMailList();
        UpdatePreviewMail(emailList.Find((_ => true)));
    }

    private void OnDestroy()
    {
        GameEvents.OnInfoMarkerAppear -= OnInfoMarkerAppear;
        GameEvents.OnEmailClicked -= ChangeEmailPreview;
        GameEvents.OnBullshitMarkerAppear -= OnBullshitMarkerAppear;
    }

    private void ChangeEmailPreview(EmailData newPreviewMail)
    {
        UpdatePreviewMail(newPreviewMail);
    }

    private void OnInfoMarkerAppear(InfoMarker marker)
    {
        if (marker.Feature != Channel.EMAIL) return;
        emailList.Insert(0, new EmailData(marker));
        UpdateMailList();
    }
    
    private void OnBullshitMarkerAppear(BullshitMarker marker)
    {
        if (marker.Feature != Channel.EMAIL) return;
        emailList.Insert(0, new EmailData(marker));
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
    
    private void UpdatePreviewMail(EmailData emailData)
    {
        Header.text = emailData.Header;
        Body.text = emailData.Body;
        Sender.text = emailData.SenderName;
    }
}
