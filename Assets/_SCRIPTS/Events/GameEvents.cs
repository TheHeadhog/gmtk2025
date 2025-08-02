using System;
using DefaultNamespace;

public static class GameEvents
{
    public static event Action<int> GameTimeChanged;
    public static event Action<CheckResponse> OnBadResponse;
    public static event Action<CheckResponse> OnGoodResponse;
    public static event Action<InfoMarker> OnInfoMarkerAppear;
    public static event Action<BullshitMarker> OnBullshitMarkerAppear;

    public static void RaiseGameTimeChanged(int gameTime) => GameTimeChanged?.Invoke(gameTime);
    public static void RaiseBadResponse(CheckResponse response) => OnBadResponse?.Invoke(response);
    public static void RaiseGoodResponse(CheckResponse response) => OnGoodResponse?.Invoke(response);
    public static void RaiseInfoMarkerAppear(InfoMarker marker) => OnInfoMarkerAppear?.Invoke(marker);
    public static void RaiseBullshitMarkerAppear(BullshitMarker marker) => OnBullshitMarkerAppear?.Invoke(marker);
}