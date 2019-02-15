using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;


public class RoomResolveData : Sing<RoomResolveData>
{
    public long m_ServerTs = 0;
    private int m_HearBeatTime = 0;


    #region Receive Room Data
    public void ResolveDataInfo(byte[] buffer)
    {
        CMD_Head head = ReadHead(buffer);
        //Debug.Log("ResolveDataInfo Json:"+head.body.data+"\n API = " + head.api);
        if (head == null)
            return;
        if (head.api != 0x00010002 && head.api != 0x00010003)
        {
            Debug.Log("len = " + head.length + "  api = " + head.api + "   datalen = " + head.body.dataSize + " data = " + head.body.data);
        }

        switch (head.api)
        {
            case 0x00010001:        //   65537
                {
                    if (head.body.type != 1)
                    {
                        return;
                    }
                    //DoSomeThing......
                   
                }

                break;
            case 0x00010002:        // 65538
                {
                    //DoSomeThing......
                }
                break;
            case 0x00010003:        //服务器推送 65539   0x00010003
                {
                    if (head.body.type != 1)
                        return;
                    try
                    {
                       //DoSomeThing......
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("推送数据错误 = " + ex);
                    }

                }
                break;
            case 0x00010004:        //心跳  65540
                {
                    RequestResult<HeartBeat> data = new RequestResult<HeartBeat>();
                    JsonUtility.FromJsonOverwrite(head.body.data, data);
                    if (data == null || data.result == null)
                        return;
                    m_ServerTs = data.result.ts;
                    ServerTimer.Instance.UpdateServerTimeBaseline(data.result.ts);
                    Debug.Log("收到心跳响应 ts = " + m_ServerTs);
                }
                break;
            case 0x00010005:        //退出房间 65541
                {
                   
                }
                break;
        }
    }
    #endregion

    //掉线重连
    public void SendLogonByUserID()
    {
        try
        {
            CMD_Head head = new CMD_Head();
            head.head = 0x3A9FDB18;
            // head.magic = GameMain.Instance.m_NetworkManager.magic;
            head.api = 0x00010001;
            head.version = 100;
            head.identifier = 0;
            head.body.type = 2;
            head.body.compress = 0;
            head.body.data = Global.instance.m_UserSession;
            head.body.dataSize = head.body.data.Length;
            head.length = Define.HEAD_Add + head.body.dataSize;
            WriteHead(head);
        }
        catch (Exception ex)
        {
            Debug.Log("掉线重连"+ ex.Message);
            //GameMain.Instance.m_Canvas.ShowOne("进入房间:" + ex.Message, null);
        }
    }

    //发送心跳消息
    public void SendHearBeat()
    {
        try
        {
            CMD_Head head = new CMD_Head();
            head.head = 0x3A9FDB18;
            // head.magic = GameMain.Instance.m_NetworkManager.magic;
            head.api = 0x00010004;
            head.version = 100;
            head.identifier = 0;
            head.body.type = 2;
            head.body.compress = 0;
            head.body.data = Global.instance.m_UserSession;
            head.body.dataSize = head.body.data.Length;
            head.length = Define.HEAD_Add + head.body.dataSize;
            WriteHead(head);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void Send_ExitGame()     //退出游戏
    {

    }

    public void ReConnectClient()
    {
        Debug.Log("发送重连消息" + "count = " + NetworkManager.count);
        SendLogonByUserID();
    }

    //读取消息
    public CMD_Head ReadHead(byte[] buffer)
    {
        if (buffer == null)
            return null;
        ByteBuffer buf = new ByteBuffer(buffer);
        CMD_Head head = new CMD_Head();
        head.head = buf.ReadInt();
        head.length = buf.ReadInt();
        head.magic = buf.ReadInt();
        head.api = buf.ReadInt();
        head.version = buf.ReadShort();
        head.identifier = buf.ReadInt();
        head.body.type = buf.ReadShort();
        head.body.compress = buf.ReadShort();
        head.body.dataSize = buf.ReadInt();
        head.body.data = buf.ReadString(head.body.dataSize);
        return head;
    }

      public void WriteHead(CMD_Head head)
    {
        if (head == null)
            return;
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInt(head.head);
        buffer.WriteInt(head.length);
        buffer.WriteInt(head.magic);
        buffer.WriteInt(head.api);
        buffer.WriteShort(head.version);
        buffer.WriteInt(head.identifier);
        buffer.WriteShort(head.body.type);
        buffer.WriteShort(head.body.compress);
        buffer.WriteInt(head.body.dataSize);
        buffer.WriteString(head.body.data);

        byte[] bytes = buffer.ToBytes();
        ////string s = "";
        ////foreach(byte b in bytes)
        ////{
        ////    s += b + "_";
        ////}
        ////Debug.Log(s);
        // GameMain.Instance.m_NetworkManager.SendMessage(bytes, 0, bytes.Length, 10000);
    }

    public void RecordMsg(string s)
    {
        string code = GetCurTime() + "----" + s +  "\r\n";
    }
    
    public static string GetCurTime()
    {
        string s = string.Empty;
        s += DateTime.Now.Day.ToString() + "/";
        s += DateTime.Now.Hour.ToString() + ":";
        s += DateTime.Now.Minute.ToString() + ":";
        s += DateTime.Now.Second.ToString() + ":";
        s += DateTime.Now.Millisecond.ToString();
        return s;
    }
}