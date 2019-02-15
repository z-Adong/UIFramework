/**
 * Created by zhang on 2019/1/26
 */
using System;
using System.Text;
using System.Security.Cryptography;

public class Define{


	public const int HEAD_LEN = 8;//头长度
    public const int HEAD_Add = 22;
    public const int SOCKET_PACKAGE = 2048 * 8;//缓冲区的大小
    public const int SOCKET_OUTTIME = 40000;//接收超时
 	public static int GetCmdDataLen(byte[] data)
    {
        ByteBuffer buf = new ByteBuffer(data);
        buf.ReadInt();
        int len = buf.ReadInt();
        return len;
    }
	public static string GetCurTime()
    {
        string s = string.Empty;
        s += DateTime.Now.Hour.ToString() + "点";
        s += DateTime.Now.Minute.ToString() + "分";
        s += DateTime.Now.Second.ToString() + "秒";
        s += DateTime.Now.Millisecond.ToString() + "豪秒";
        return s;
    }
	public static string TimeStamp()//Unix时间戳
    {
        DateTime d1 = Convert.ToDateTime("1970-1-1 00:00:00");
        DateTime d2 = DateTime.Now;
        TimeSpan t = d2 - d1;
        return Convert.ToInt32(t.TotalSeconds).ToString();
    }
    /// <summary>  
    /// 获取时间戳
    /// </summary>  
    /// <param name=”timeStamp”></param>  
    /// <returns></returns>  
    public static long GetTimeStamp(DateTime dt)
    {
        DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        long timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
        return timeStamp;
    }
    /// <summary>  
    /// 时间戳(毫秒)
    /// </summary>  
    /// <param name=”time”></param>  
    /// <returns></returns>  
    public static long GetMilliseconds(DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (long)(time - startTime).TotalMilliseconds;
    }
	/// <summary>  
    /// 时间戳转为C#格式时间  
    /// </summary>  
    /// <param name=”timeStamp”></param>  
    /// <returns></returns>  
    public static DateTime GetTime(long timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp.ToString());
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime targetDt = dtStart.Add(toNow);
        return targetDt;
    }
    //字符串转MD5
    public static string StringToMD5(string _url)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = Encoding.UTF8.GetBytes(_url);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();
        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString.ToLower();
    }
    //判断Texture类型
    public static string GetTextureType(byte[] data)
    {
        string type = null;
        if (data[1] == 'P' && data[2] == 'N' && data[3] == 'G')
        {
            type = "PNG";
            return type;
        }
        if (data[0] == 'G' && data[1] == 'I' && data[2] == 'F')
        {
            type = "GIF";
            return type;
        }
        int length = data.Length;
        //$ff, $d8
        if (data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
        {//遇到有文件的最后一位是00，所以不能以FFD9来判断结束
            type = "JPG";
            return type;
        }
        return type;
    }
}

public class Sing<T> where T : class, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }
}

