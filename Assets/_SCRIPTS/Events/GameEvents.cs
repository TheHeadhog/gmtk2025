using System;
using DefaultNamespace;

public static class GameEvents
{
    public static event Action<int> GameTimeChanged;
    public static event Action<CheckResponse> OnBadResponse;
    public static event Action<CheckResponse> OnGoodResponse;
    public static event Action<InfoMarker> OnInfoMarkerAppear;
    public static event Action<BullshitMarker> OnBullshitMarkerAppear;
    public static event Action<EmailData> OnEmailClicked;
    public static event Action<NewsData> OnNewsButtonClicked;

    public static void RaiseGameTimeChanged(int gameTime) => GameTimeChanged?.Invoke(gameTime);
    public static void RaiseBadResponse(CheckResponse response) => OnBadResponse?.Invoke(response);
    public static void RaiseGoodResponse(CheckResponse response) => OnGoodResponse?.Invoke(response);
    public static void RaiseInfoMarkerAppear(InfoMarker marker) => OnInfoMarkerAppear?.Invoke(marker);
    public static void RaiseBullshitMarkerAppear(BullshitMarker marker) => OnBullshitMarkerAppear?.Invoke(marker);
    public static void RaiseEmailClicked(EmailData emailData)=> OnEmailClicked?.Invoke(emailData);
    public static void RaiseNewsButtonClicked(NewsData newsData) => OnNewsButtonClicked?.Invoke(newsData);
}