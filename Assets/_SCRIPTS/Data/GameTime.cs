using System;
using UnityEngine;

[Serializable]
public struct GameTime
{
    [Range(9, 17)] public int Hours;
    [Range(0, 59)] public int Minutes;

    public GameTime(int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }
}

public static class GameTimeExtensions
{
    public static int ToGameTick(this GameTime gameTime)
    {
        return (gameTime.Hours - 9) * 60 + gameTime.Minutes;
    }

    public static GameTime FromGameTick(this int gameTick)
    {
        int hours = gameTick / 60 + 9;
        int minutes = gameTick % 60;
        return new GameTime(hours, minutes);
    }

    public static bool EqualsTo(this GameTime gameTime1, GameTime gameTime2)
    {
        return gameTime1.Hours.Equals(gameTime2.Hours) &&
               gameTime1.Minutes.Equals(gameTime2.Minutes);
    }
}