using UnityEngine;
using System.Collections;
using System.IO;
using System;


public class FileTools : MonoBehaviour
{

    // 不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    //    public static readonly string PathURL =
    // #if UNITY_ANDROID || UNITY_STANDALONE_WIN  //安卓
    // "jar:file://" + Application.dataPath + "!/assets/";
    // #elif UNITY_IPHONE  //iPhone
    //    Application.dataPath + "/Raw/";
    // #elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
    //    "file://" + Application.dataPath + "/StreamingAssets/";
    // #else
    //    string.Empty;
    // #endif

    //文本中每行的内容
    public ArrayList infoall;


    // void Start()
    // {
    //     print("当前文件路径:"+Application.persistentDataPath);
    //     //删除文件
    //     DeleteFile(Application.persistentDataPath,"FileName.txt");
    //     //创建文件，共写入2次数据
    //     CreateFile(Application.persistentDataPath,"FileName.txt","test0,123");
    //     CreateFile(Application.persistentDataPath, "FileName.txt", "test1,123");

    //     //得到文本中每一行的内容
    //     infoall = LoadFile(Application.persistentDataPath,"FileName.txt");
    //     foreach (string s in infoall)
    //     {
    //        Debug.Log(s);
    //     }
    // }



    /**
    * path：文件创建目录
    * name：文件的名称
    * info：写入的内容
    */
    public void CreateFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            sw = t.CreateText();//如果此文件不存在则创建
        }
        else
        {
            sw = t.AppendText();//如果此文件存在则打开
        }

        sw.WriteLine(info);//以行的形式写入信息 
        sw.Close();//关闭流
        sw.Dispose();//销毁流
    }

    /**
    * 读取文本文件
    * path：读取文件的路径
    * name：读取文件的名称
    */
    public ArrayList LoadFile(string path, string name)
    {

        StreamReader sr = null;//使用流的形式读取
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;//路径与名称未找到文件则直接返回空
        }

        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);//一行一行的读取 将每一行的内容存入数组链表容器中
        }

        sr.Close();//关闭流
        sr.Dispose();//销毁流
        return arrlist;//将数组链表容器返回
    }



    public void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);
    }
}
