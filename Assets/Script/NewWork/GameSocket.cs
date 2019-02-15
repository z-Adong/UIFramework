using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

class GameSocket
{
    private Socket clientSocket = null;

    bool mConnected;//连接是否可用

    public GameSocket()
    {
        SetConnect(false);
    }


    //连接是否可用
    void SetConnect(bool bFlag)
    {
        mConnected = bFlag;
    }

    /// <summary>
    /// 设置连接
    /// </summary>
    /// <param IP地址="ip"></param>
    /// <param 端口="port"></param>
    /// <returns></returns>
    public bool Connect(string ip, int port)
    {
        try
        {
            string serverIp = ip;
            //AddressFamily ipType = AddressFamily.InterNetwork;
            IPAddress[] list = Dns.GetHostAddresses(serverIp);
            IPAddress address = list[0];
            clientSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);//实例化一个客户端的网络端点
            //clientSocket = new Socket(ipType, SocketType.Stream, ProtocolType.Tcp);//实例化一个客户端的网络端点

            //IPAddress address = IPAddress.Parse(serverIp);//将IP地址字符串转换为IP地址实例
            IPEndPoint point = new IPEndPoint(address, port);//设置IP和端口
            clientSocket.Connect(point);//连接服务器


            if (clientSocket.Connected)
            {
                clientSocket.ReceiveBufferSize = Define.SOCKET_PACKAGE;//接收缓冲区的大小
                clientSocket.SendBufferSize = Define.SOCKET_PACKAGE;//接收缓冲区的大小
                clientSocket.ReceiveTimeout = Define.SOCKET_OUTTIME;//接收超时
                clientSocket.NoDelay = true;//无延迟
                SetConnect(true);
            }

            return true;
        }
        catch (SocketException e)
        {
            int errorCode = (int)e.SocketErrorCode;
            SetConnect(false);
            return false;
        }
    }


    /// <summary>
    /// 发送SOCKET消息
    /// </summary>
    /// <param 字节="buffer"></param>
    /// <param name="offset"></param>
    /// <param 大小="size"></param>
    /// <returns></returns>
    public bool Send(byte[] buffer, int offset, int size)
    {
        try
        {
            if (clientSocket == null)
            {
                //NetworkManager.mRecord.Enqueue("socket send error = clientSocket is null!");
                return false;
            }
            if (!clientSocket.Connected)
            {
                //NetworkManager.mRecord.Enqueue("socket send error = clientSocket is not connect!");
                return false;
            }
            clientSocket.Send(buffer, offset, size, SocketFlags.None);
            return true;
        }
        catch (Exception e)
        {
            SetConnect(false);
            //NetworkManager.mRecord.Enqueue("socket send error = " + e.Message);
            return false;
        }
    }

    private byte[] tempcon = null;
    //从服务器端接受返回信息
    public List<byte[]> OnReceive(byte[] recedata, int length)
    {

        try
        {
            List<byte[]> outList = new List<byte[]>();
            if (tempcon != null && tempcon.Length > 0)
            {
                byte[] temp = new byte[recedata.Length];
                Buffer.BlockCopy(recedata, 0, temp, 0, recedata.Length);
                recedata = new byte[temp.Length + tempcon.Length];
                Buffer.BlockCopy(tempcon, 0, recedata, 0, tempcon.Length);
                Buffer.BlockCopy(temp, 0, recedata, tempcon.Length, temp.Length);

                length += tempcon.Length;
                tempcon = null;

            }

            int start = 0;
            byte[] data = new byte[length];
            Buffer.BlockCopy(recedata, 0, data, 0, length);
            while (length - start >= Define.HEAD_LEN)
            {
                int size = Define.GetCmdDataLen(data) + 8;
                if (size <= 8)
                    break;

                if (size > length - start)
                {
                    int addlen = length - start;
                    tempcon = new byte[addlen];
                    Buffer.BlockCopy(recedata, start, tempcon, 0, addlen);
                    break;
                }
                else
                {
                    byte[] temp = new byte[size];
                    Buffer.BlockCopy(recedata, start, temp, 0, size);
                    lock (NetworkManager.objLock)
                    {
                        outList.Add(temp);
                    }
                    start += size;
                    data = new byte[length - start];
                    Buffer.BlockCopy(recedata, start, data, 0, length - start);
                }
            }

            if (length - start < Define.HEAD_LEN)
            {
                tempcon = new byte[length - start];
                Buffer.BlockCopy(recedata, start, tempcon, 0, length - start);
            }

            return outList;
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message + " errorcode =" + e.ErrorCode);
            return null;
        }

    }

    //接收指定客户端Socket的消息
    public int Recv(byte[] recvByte)
    {
        try
        {
            if (clientSocket == null)
            {
                Debug.Log("Socket is null");
                return -1;
            }

            int n = clientSocket.Receive(recvByte);//从服务器端接受返回信息
            return n;
        }
        catch (SocketException e)
        {
            int errorCode = (int)e.SocketErrorCode;

            if (!IsError(errorCode))
                return 0;

            SetConnect(false);

            if (errorCode == 10054 || errorCode == 10052)
                return -2;
            else if (errorCode == 10057 || errorCode == 10051 || errorCode == 10050)
                return -3;
            else if (errorCode == 10060 || errorCode == 10035)
                return -4;

            return -1;
        }
    }

    //是否出错
    bool IsError(int nError)
    {
        if (nError == 10053 || nError == 10038 || nError == 10004)
            return false;
        return true;
    }

    //关闭Sokect
    public void Close()
    {
        try
        {
            if (clientSocket == null)
                return;
            clientSocket.Shutdown(SocketShutdown.Both);

            clientSocket.Close();
            clientSocket = null;

            SetConnect(false);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
            clientSocket = null;
        }

    }

    public bool IsConnect()
    {
        return mConnected;
    }

}

