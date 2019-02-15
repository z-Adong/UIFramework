using UnityEngine;
using System.Collections;

public class ServerTimer : MonoBehaviour
{
    private long serverTimeBaseline;
    private long timeOffset;
    private bool timeReady = false;
    private long timeDifference;

    private static ServerTimer instance = null;
    public static ServerTimer Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateServerTimeBaseline(long serverTimeBaseline)
    {
        //CancelInvoke("Running");
        this.serverTimeBaseline = serverTimeBaseline;
        timeDifference = serverTimeBaseline - Define.GetMilliseconds(System.DateTime.Now);
        //timeOffset = 0;
        if (!timeReady)
        {
            timeReady = true;
        }
        //StartServerTimer();
    }

    public void Stop()
    {
        //CancelInvoke("Running");
        //timeReady = false;
    }

    public long GetCurrentServerTime()
    {
        return Define.GetMilliseconds(System.DateTime.Now) + timeDifference;
        //return serverTimeBaseline + timeOffset*100;
    }

    public bool IsReady()
    {
        return timeReady;
    }

    private void StartServerTimer()
    {
        InvokeRepeating("Running", 0.1f, 0.1f);
    }

    private void Running()
    {
        timeOffset++;
    }
}