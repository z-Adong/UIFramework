using System;
using System.Collections.Generic;

public enum NetEventError
{
    connected = 0,              //链接成功
    disconnect,                 //关闭网络链接
    connectedFialed,            //链接失败
}
public enum NetState
{
    disconnect,
    connectIng,//断连
    connected,//连接正常
    shutdown,//Socket挂掉了

}
public class NetController
{
    GameSocket mSocket;
    public string mIp;//服务器IP地址
    public int mPort;//服务器端口
    public NetState mNetState = NetState.disconnect;//Socket状态
    bool mNeedReconnect = true;//是否需要重新连接

    long mCurrtime;
    int mMaxReconnectTime = 15;

    public NetController()
    {
        mSocket = new GameSocket();
    }

    /// <summary>
    /// 控制链路
    /// </summary>
    /// <param IP地址="ip"></param>
    /// <param 端口="nPort"></param>
    public void Connect(string ip, int nPort)
    {
        //CloseSocket();
        mSocket = new GameSocket();
        mNeedReconnect = true;
        mCurrtime = GetTotalSeconds() - mMaxReconnectTime;
        mIp = ip;
        mPort = nPort;
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
        if (mSocket == null || buffer == null)
        {
            return false;
        }
        return mSocket.Send(buffer, offset, size);
    }

    //设置连接
    public void SetReconnect(bool bFlag)
    {
        mNeedReconnect = bFlag;

        CloseSocket();
        //立刻断线重连
        if (mNeedReconnect)
        {
            mCurrtime = GetTotalSeconds() - mMaxReconnectTime;
            mSocket = new GameSocket();
        }
    }

    //连接状态
    public void Update()
    {
        try
        {
            switch (mNetState)
            {
                case NetState.disconnect:
                    {
                        long nTme = (GetTotalSeconds() - mCurrtime);

                        if (mNeedReconnect && nTme >= mMaxReconnectTime)
                        {
                            if (string.IsNullOrEmpty(mIp) || mPort == 0)
                                return;
                            mCurrtime = GetTotalSeconds();
                            mNetState = NetState.connectIng;
                            return;
                        }
                    }
                    break;

                case NetState.connectIng://断连
                    {
                        ProcessConnect();//重新连接
                        mNetState = NetState.connected;//连接正常
                    }
                    break;

                case NetState.connected://连接正常
                    {
                        //RoomResolveData.Instance.SendLogonByUserID();
                        
                        try
                        {
                            byte[] recvData = new byte[Define.SOCKET_PACKAGE];
                            int nLen = mSocket.Recv(recvData);
                            if (nLen == 0)
                                return;
                            if (nLen < 0)
                            {
                                if (nLen == -2)
                                {
                                    NetworkManager.AddError((int)NetWorkError.breakLine_Server);
                                }
                                else if (nLen == -3)
                                {
                                    mCurrtime = GetTotalSeconds();
                                    mNetState = NetState.shutdown;
                                }
                                else
                                {
                                    NetworkManager.AddError((int)NetWorkError.breakLine_Client);
                                }

                                NetworkManager.AddError((int)NetWorkError.socket_error);
                                return;
                            }

                            List<byte[]> recvList = mSocket.OnReceive(recvData, nLen);
                            if (recvList == null || recvList.Count == 0)
                                return;
                            for (int i = 0; i < recvList.Count; i++)
                            {
                                lock (NetworkManager.objLock)
                                    NetworkManager.AddEvent(recvList[i]);
                            }
                        }
                        catch(Exception e)
                        {
                            NetworkManager.AddError((int)NetWorkError.breakLine_Client);
                        }
                    }
                    break;

                case NetState.shutdown://Socket挂掉了
                    {
                        CloseSocket();//关闭Socket
                    }
                    break;
            }
        }
        catch(Exception e)
        {
            NetworkManager.AddError((int)NetWorkError.breakLine_Client);
        }
    }


    //连接设置
    void ProcessConnect()
    {
        mSocket.Connect(mIp, mPort);
        if (mSocket.IsConnect())
        {
            
            RoomResolveData.Instance.SendLogonByUserID();
            NetworkManager.AddError((int)NetWorkError.connected);
        }
        else
        {
            NetworkManager.AddError((int)NetWorkError.disconnect);
        }
    }

    //关闭Socket
    public void CloseSocket()
    {
        if (mSocket != null)
            mSocket.Close();

        if (NetworkManager.mEventQueue != null)
            NetworkManager.mEventQueue.Clear();//清空聊天室信息队列

        mNetState = NetState.disconnect;
    }

    //时间刻度
    public long GetTotalSeconds()
    {
        TimeSpan t = new TimeSpan(DateTime.Now.Ticks);
        return (long)t.TotalSeconds;
    }

    public bool SocketStadus()
    {
        
        if (mSocket != null)
        {
            return mSocket.IsConnect();
        }
        return false;
    }
}
