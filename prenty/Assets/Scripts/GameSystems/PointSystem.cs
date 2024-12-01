using System;
using UnityEngine;

public class PointSystem : MonoBehaviour, IGameSystem
{

    public int pointCounter;
    public TimeSpan timeSinceBegin {get; private set;}
    public Boolean isTimeRunning;

    public void Initialize()
    {
        pointCounter = 0;
        timeSinceBegin = TimeSpan.Zero;

        StartCoroutine(GameTimer());
        isTimeRunning = true;
    }

    public void Uninitialize()
    {
        StopCoroutine(GameTimer());
        isTimeRunning = false;

    }
   
    public void AddPoint()
    {
        pointCounter += 1;
    }

    private System.Collections.IEnumerator GameTimer()
    {
        DateTime startTime = DateTime.UtcNow;

        while (isTimeRunning)
        {
            timeSinceBegin = DateTime.UtcNow - startTime;
            yield return null;
        }
    }
    
}
