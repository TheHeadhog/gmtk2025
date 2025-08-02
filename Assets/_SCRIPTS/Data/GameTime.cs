using UnityEngine;

public class GameTime : ScriptableObject
{
    [Range(9, 17)]
    public int Hours;
    [Range(0, 59)]
    public int Minutes;
}

public static class GameTimeExtensions
{
    public static int ToGameTick(this GameTime gameTime)
    {
        return (gameTime.Hours - 9) * 60 + gameTime.Minutes;
    }
}
