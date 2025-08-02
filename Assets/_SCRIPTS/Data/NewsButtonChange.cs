using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsButtonChange : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Header;
    [SerializeField]
    private TMP_Text Body;
    [SerializeField]
    private Image Button1;
    [SerializeField]
    private Image Button2;
    [SerializeField]
    private Image Button3;
    [SerializeField] 
    private Sprite FilledSprite;
    [SerializeField] 
    private Sprite EmptySprite;
    
    private List<NewsData> newsData = new();
    private int CurrentTabNumber;

    private void Awake()
    {
        newsData.Add(new NewsData("No news", "There are currently no news", 1));
        newsData.Add(new NewsData("No news", "There are currently no news", 2));
        newsData.Add(new NewsData("No news", "There are currently no news", 3));

        CurrentTabNumber = 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeNewsPreview(newsData[0]);
        GameEvents.OnNewsButtonClicked += ChangeNewsPreview;
        GameEvents.OnInfoMarkerAppear +=  OnInfoMarkerAppeared;
        GameEvents.OnBullshitMarkerAppear += OnBullshitMarkerAppeared;
    }

    void OnDestroy()
    {
        GameEvents.OnNewsButtonClicked -= ChangeNewsPreview;
        GameEvents.OnInfoMarkerAppear -= OnInfoMarkerAppeared;
        GameEvents.OnBullshitMarkerAppear -= OnBullshitMarkerAppeared;
    }

    private void ChangeNewsPreview(NewsData newsData)
    {
        ChangeNewsDisplayText(newsData.Header, newsData.Body);
        HighlightProperButton(newsData.TabNumber);
    }

    private void ChangeNewsDisplayText(string headerText, string bodyText)
    {
        Header.text = headerText;
        Body.text = bodyText;
    }

    private void HighlightProperButton(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case 1:
                Button1.sprite = FilledSprite;
                Button2.sprite = EmptySprite;
                Button3.sprite = EmptySprite;
                CurrentTabNumber = 1;
                break;
            case 2:
                Button1.sprite = EmptySprite;
                Button2.sprite = FilledSprite;
                Button3.sprite = EmptySprite;
                CurrentTabNumber = 2;
                break;
            case 3:
                Button1.sprite = EmptySprite;
                Button2.sprite = EmptySprite;
                Button3.sprite = FilledSprite;
                CurrentTabNumber = 3;
                break;
            default:
                Debug.LogError("Invalid button number");
                break;
        }
    }

    private void OnInfoMarkerAppeared(InfoMarker marker)
    {
        UpdateNewsDataList(marker.ReceivedNewsData.Header, marker.ReceivedNewsData.Body);
        ChangeNewsPreview(new NewsData(marker.ReceivedNewsData.Header, marker.ReceivedNewsData.Body, CurrentTabNumber));
    }
    
    private void OnBullshitMarkerAppeared(BullshitMarker marker)
    {
        UpdateNewsDataList(marker.ReceivedNewsData.Header, marker.ReceivedNewsData.Body);
        ChangeNewsPreview(new NewsData(marker.ReceivedNewsData.Header, marker.ReceivedNewsData.Body, CurrentTabNumber));
    }

    private void UpdateNewsDataList(string header, string body)
    {
        newsData.Add(new NewsData(header, body, 4));
        newsData.RemoveAt(0);
        foreach (var newsDataPiece in newsData)
        {
            newsDataPiece.TabNumber -= 1;
        }
    }

    public void OnButton1Clicked()
    {
        GameEvents.RaiseNewsButtonClicked(newsData[0]);
    }
    
    public void OnButton2Clicked()
    {
        GameEvents.RaiseNewsButtonClicked(newsData[1]);
    }
    
    public void OnButton3Clicked()
    {
        GameEvents.RaiseNewsButtonClicked(newsData[2]);
    }
}
