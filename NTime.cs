using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class NTime
{
    public static float startTime = 0;

    public static int FixedFrameCount = 0;
    public static int FrameCount = 0;
    public static float Time = 0;


    static bool pauseGame;
    public static bool PauseGame
    {
        set
        {
            if (value)
                UnityEngine.Time.timeScale = 0;
            else
                UnityEngine.Time.timeScale = 1;
            if (pauseGame != value)
                onPauseGame?.Invoke(value);
            pauseGame = value;
        }
        get => pauseGame;
    }
    public static Action<bool> onPauseGame;
    public static int FixedTimeFrameDifference(int _frame)
    {
        return FixedFrameCount - _frame;
    }
    public static int TimeFrameDifference(int _frame)
    {
        return FrameCount - _frame;
    }
    public static float TimeDifference(float _time)
    {
        return Time - _time;
    }
    public static string FFCToString
    {
        get
        {
            return string.Format("<color=green>{0}</color> <color=red>{1}</color>,", $"FixedFrameCount:{FixedFrameCount}", $"FrameCount:{FrameCount} ");
        }
    }



    public static void Log(object message)
    {
        Debug.Log("This is a Log:" + message.ToString() + FFCToString);
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning("This is a Warning Log:" + message.ToString() + FFCToString);
    }
    public static void LogError(object message)
    {
        Debug.LogError("This is an Error Log:" + message.ToString() + FFCToString);
    }
}
