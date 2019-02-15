using System;
using System.Threading;


public class NetThreadController
{

    public bool mStop = true;//当前线程是否可用
    Thread mThread;//当前线程

    public NetController mNetController = null;

    static NetThreadController mInstance;

    public static NetThreadController Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new NetThreadController();
            return mInstance;
        }
    }

    public NetThreadController()
    {
        mNetController = new NetController();
    }

    /// <summary>
    /// 线程链路
    /// </summary>
    /// <param IP地址="ip"></param>
    /// <param 端口="nPort"></param>
    /// <returns></returns>
    public bool Connect(string ip, int nPort)
    {
        if (mNetController == null)
        {
            return false;
        }
        mNetController.Connect(ip, nPort);
        return true;
    }

    //开启并运行一个线程 -> ThreadFunc
    public void StartThread()
    {
        mThread = new Thread(ThreadFunc);
        mThread.Start(this);
    }

    //线程ThreadFunc
    static void ThreadFunc(Object o)
    {
        NetThreadController controller = (NetThreadController)o;
        while (controller.IsStop())//线程运行时
        {
            controller.Update();//执行当前线程更新
            Thread.Sleep(5);//将当前线程挂起5毫秒  时间结束后 继续执行下一步  和yield类似
        }
    }

    public void Update()
    {
        if (mNetController == null)
            return;
        mNetController.Update();
    }

    /// <summary>
    /// 设置连接
    /// </summary>
    /// <param 是否连接="bFlag"></param>
    public void SetReconnect(bool bFlag)
    {
        if (mNetController == null)
            return;
        mNetController.SetReconnect(bFlag);
    }


    /// <summary>
    /// 发送SOCKET消息
    /// </summary>
    /// <param 字节="buffer"></param>
    /// <param name="offset"></param>
    /// <param 大小="size"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public bool Send(byte[] buffer, int offset, int size, int timeout)
    {
        if (mNetController != null)
            return mNetController.Send(buffer, offset, size, timeout);
        return false;
    }

    //线程是否可用
    public bool IsStop()
    {
        return mStop;
    }

    //关闭Socket
    public void CloseSocket()
    {
        if (mNetController != null)
            mNetController.CloseSocket();
    }

    //停止线程
    public void StopThread()
    {
        mStop = false;
        Thread.Sleep(1000); //将当前线程挂起1000毫秒  时间结束后 继续执行下一步  和yield类似
        try
        {
            if (mThread.IsAlive) //线程当前是否为可用状态
                mThread.Interrupt();//可用时中断线程
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    //Net状态
    public NetState CurState
    {
        get
        {
            if (mNetController != null)
                return mNetController.mNetState;
            return NetState.disconnect;
        }
    }
}

