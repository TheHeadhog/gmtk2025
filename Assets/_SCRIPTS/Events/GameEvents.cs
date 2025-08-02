using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<int> GameTimeChanged;

    public static void RaiseGameTimeChanged(int gameTime) => GameTimeChanged?.Invoke(gameTime);
}