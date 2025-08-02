using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameTime")]
public class GameTime : ScriptableObject
{
    [Range(9, 17)] public int Hours;
    [Range(0, 59)] public int Minutes;
}

public static class GameTimeExtensions
{
    public static int ToGameTick(this GameTime gameTime)
    {
        return (gameTime.Hours - 9) * 60 + gameTime.Minutes;
    }

    public static bool EqualsTo(this GameTime gameTime1, GameTime gameTime2)
    {
        return gameTime2 != null && gameTime1.Hours.Equals(gameTime2.Hours) &&
               gameTime1.Minutes.Equals(gameTime2.Minutes);
    }
}