using System;
using DefaultNamespace;

public static class GameEvents
{
    public static event Action<int> GameTimeChanged;
    public static event Action<CheckResponse> OnBadResponse;
    public static event Action<CheckResponse> OnGoodResponse;

    public static void RaiseGameTimeChanged(int gameTime) => GameTimeChanged?.Invoke(gameTime);
    public static void RaiseBadResponse(CheckResponse response) => OnBadResponse?.Invoke(response);
    public static void RaiseGoodResponse(CheckResponse response) => OnGoodResponse?.Invoke(response);
}