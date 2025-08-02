using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailBehavior : MonoBehaviour
{
    private List<EmailData> emailList = new List<EmailData>()
    {
        new EmailData("MAIL HEADER1","This is mail body 1"),
        new EmailData("MAIL HEADER2","This is mail body 2"),
        new EmailData("MAIL HEADER3","This is mail body 3"),
    };
    private int CurrentlySelectedEmailId = 0;
    [SerializeField] 
    private GameObject emailListGameObject;
    [SerializeField]
    private TMP_Text Header;
    [SerializeField]
    private TMP_Text Body;

    [SerializeField] private GameObject EmailListItemPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        Header.text = emailList[CurrentlySelectedEmailId].Header;
        Body.text = emailList[CurrentlySelectedEmailId].Body;
    }
}
