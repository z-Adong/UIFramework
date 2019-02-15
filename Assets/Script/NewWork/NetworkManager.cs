using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum NetWorkError
{
    socket_error,//
    disconnect,//�Ͽ�״̬
    connected, //����״̬
    breakLine_Client,//�ͻ����ر�Socket
    breakLine_Server,//�������ر�Socket
    reConnect_Server,//����������
    reConnect_Client,//�����ͻ���
}

public class NetworkManager : MonoBehaviour
{
    /// <summary>
    /// Queue���� -> ԭ������������������ģʽ�������г��в���
    /// Enqueue ����
    /// Dequeue �Ӷ�����ȡ��
    /// </summary>
    public static Queue<byte[]> mEventQueue = new Queue<byte[]>();//��������Ϣ
    public static Queue<int> mError = new Queue<int>();//�������Ͷ���
    public static Queue<string> mRecord = new Queue<string>();//������־����
    public static int m_ReConnectCount = 0;  

    private int mErrorType = 0;//��������
    private bool bReConecting = false;//������
	private bool bBreakLine = false;//�Ƿ����
    public static object objLock = new object();

    //magic
    public int magic = 0x6C32A187;
    //������IP��ַ
    public string serverIP = "120.26.204.58";
    //�������˿�
    public int port = 33000; 
    //URLƴ��
    public string portName = "";
    public bool netStatus=false;


    void Start()    
    {
        StartCoroutine(NetUpdate());
        NetThreadController.Instance.mStop = true;
        NetThreadController.Instance.StartThread();   
    }

    /// <summary>
    /// ��������Ϣ����
    /// </summary>
    /// <param �ֽ�="data"></param>
    public static void AddEvent(byte[] data)
    {
        mEventQueue.Enqueue(data);
    }

    /// <summary>
    /// ������Ϣ����
    /// </summary>
    /// <param ��������="type"></param>
    public static void AddError(int type)
    {
        mError.Enqueue(type);
    }

    //������Ϣ����
    IEnumerator NetUpdate()
    {
        while(true)
        {
            while (mRecord.Count > 0)
            {
                RoomResolveData.Instance.RecordMsg(mRecord.Dequeue());
            }
            while (mEventQueue.Count > 0)
            {
                byte[] message = mEventQueue.Dequeue();
				RoomResolveData.Instance.ResolveDataInfo(message);
            }
            while (mError.Count > 0)
            {
                int type = mError.Dequeue();
                ReceiveError(type);//���������Ϣ
            }
            yield return 0;
            continue;
        }       
    }

    //��������״̬
    void LateUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)//����������
        {          
            if (!bBreakLine)//�Ƿ����
            {
                Close();
                bBreakLine = true;
				RoomResolveData.Instance.RecordMsg("��ǰ���粻���û������ѶϿ�");			   
				//SceneGlobal.Instance.m_Canvas.ShowTwo("��ǰ���粻�ȶ������ȷ�ؽ����ӣ�", BreakLine_Client, null);
            }
        }
    }

    //������������
    public bool SendConnect()
    {
        return NetThreadController.Instance.Connect(serverIP, port);//����IP��ַ�Ͷ˿�
    }

    /// <summary>
    /// ����SOCKET��Ϣ
    /// </summary>
    /// <param �ֽ�="buffer"></param>
    /// <param name="offset"></param>
    /// <param ��С="size"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public bool SendMessage(byte[] buffer, int offset, int size, int timeout)
    {
        return NetThreadController.Instance.Send(buffer, offset, size, timeout);
    }

    //ֹͣ�߳�
    public void StopNetThread()
    {
        NetThreadController.Instance.StopThread();
    }

    #region ������

    public static int count = 0;

    //��������
    void ReceiveError(int type)
    {
        if (type == (int)NetWorkError.socket_error) 
        {          
            return;
        }

        if (type == (int)NetWorkError.connected)//����״̬
        {
            LineSucceed();//����ͨ��
        }
        else if (type == (int)NetWorkError.disconnect)//�Ͽ�״̬       
        {
            LineFail();//�������
        }
        else if (type == (int)NetWorkError.breakLine_Client)//�ͻ����ر�Socket
        {
            BreakL_Client();//�ͻ�������
        }
        else if (type == (int)NetWorkError.breakLine_Server)//����˹ر�Socket
        {
            BreakL_Server();//����������
        }        
        count++;
    }

    //����ͨ��
    void LineSucceed()
    {
        //     if (mErrorType == (int)NetWorkError.reConnect_Server)
        //     {
        //DisConnetNotice ();
        //RoomResolveData.Instance.RecordMsg("LineSucceed:Server" + count);
        //     }
        //     else if (mErrorType == (int)NetWorkError.reConnect_Client)
        //     {
        //RoomResolveData.Instance.SendLogonByUserID(); 
        //RoomResolveData.Instance.RecordMsg("LineSucceed:Client" + count);
        //}
        //else                                                        
        //{
        //    RoomResolveData.Instance.SendLogonByUserID();
        //}
        //RoomResolveData.Instance.SendLogonByUserID();
        bBreakLine = false;
        bReConecting = false;
        mErrorType = (int)NetWorkError.connected;
    }

    //�������
    public void LineFail()
    {
        if (mErrorType == (int)NetWorkError.reConnect_Client)
        {
            //��ʾ���ض���

            bBreakLine = true;
            m_ReConnectCount++;
            RoomResolveData.Instance.RecordMsg("LineFail:Client, m_ReConnectCount = " + m_ReConnectCount);
            if (m_ReConnectCount > 2)
            {
                NetThreadController.Instance.SetReconnect(false);
				//SceneGlobal.Instance.m_Canvas.ShowOne("��������ʧ��", ()=>{
				//	Close();
				//	//SceneGlobal.Instance.m_Canvas.CreatScanBtn();
				//});
                Debug.Log("m_ReConnectCount > 2:" + count);
                m_ReConnectCount = 0;
            }
        }
        else if (mErrorType == (int)NetWorkError.reConnect_Server)
        {
			DisConnetNotice ();
			RoomResolveData.Instance.RecordMsg("LineFail:Server"+ count);
        }
        
        bReConecting = false;                                   
    }

    //�ͻ��˶���
    void BreakL_Client()
    {
        Close();
    }

    //����������
    void BreakL_Server()
    {
        if (bReConecting)
            return;

        if (!bBreakLine)
        {
            mErrorType = (int)NetWorkError.reConnect_Server;
            NetThreadController.Instance.SetReconnect(true);
            bReConecting = true;
        }
        else
            mErrorType = (int)NetWorkError.reConnect_Client;

        RoomResolveData.Instance.RecordMsg("Server Break" + count);
    }
    #endregion

    //����֪ͨ
    private void DisConnetNotice()
	{
		//SceneGlobal.Instance.m_Canvas.ShowOne("��Ϸ�쳣", ()=>{
		//	Close();
  //          //�Ƿ���Ҫ������Ϣ���߷������������Ѿ��뿪������
		//});
	}

    //����״̬
    public void BreakLine_Client()
    {
        Debug.Log("breakLine_Client" + count);
        bBreakLine = true;
        m_ReConnectCount = 0;                                           
        mErrorType = (int)NetWorkError.reConnect_Client;

        //��ʾ���ض���

        NetThreadController.Instance.SetReconnect(true);     
        count++;
    }

    //�ر��߳�����
    public void Close()
    {
        NetThreadController.Instance.SetReconnect(false);
        StopNetThread();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void RusumeGame()
    {
        BreakLine_Client();
    }

    #region ����������

    /// <summary>
    /// ��������
    /// </summary>
    /// <param ���="repeatTime"></param>
    public void StartHearBeat(float repeatTime)
    {
        InvokeRepeating("SendHearBeat", repeatTime, repeatTime);
    }

    /// <summary>
    /// ֹͣ����
    /// </summary>
    public void StopHeartBeat()
    {
        CancelInvoke("SendHearBeat");
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void SendHearBeat()
    {
        RoomResolveData.Instance.SendHearBeat();
    }
    #endregion

    //�����˳�ʱֹͣ�߳�
    void OnDestroy()
    {
        StopNetThread();
    }
}

