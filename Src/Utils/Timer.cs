using System;
using System.Runtime.InteropServices;

public static class Timer
{
    [DllImport("kernel32.dll")]
    private static extern bool QueryPerformanceCounter (ref long lpPerformanceCount);
    [DllImport("kernel32.dll")]
    private static extern bool QueryPerformanceFrequency (ref long lpFrequency);

    private static long startCounter = -1L;

    /// <summary>
    /// 起動時の初期設定を行う
    /// </summary>
    public static void Start ()
    {
        QueryPerformanceCounter(ref startCounter);
    }

    public static double ElapsedMilliSec ()
    {
        if (startCounter == -1L) // if not initialized, yet
            Start();

        long stopCounter = 0;
        QueryPerformanceCounter(ref stopCounter);
        long frequency = 0;
        QueryPerformanceFrequency(ref frequency);
        return (double)(stopCounter - startCounter) * 1000.0 / frequency;
    }
}