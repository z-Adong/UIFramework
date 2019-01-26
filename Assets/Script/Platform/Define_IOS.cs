/**
 * Created by zhang on 2019/1/26
 */
using UnityEngine;
using System.Runtime.InteropServices;
//通信IOS
public static class Define_IOS
{
    [DllImport("__Internal")]
    private static extern void callVoid();
    [DllImport("__Internal")]
    private static extern void callString(string url);
    [DllImport("__Internal")]
    private static extern void callTwoString(string str1,string str2);
    [DllImport("__Internal")]
    private static extern int callInt();

    //无参
    public static void IOS_CallVoid()
    {
        callVoid();
    }
    //单参
    public static void iOS_callString(string str)
    {
        callString(str);
    }
    //双参
    public static void iOS_callTwoString(string str1, string str2)
    {
        callTwoString(str1, str2);
    }
    //返回值
    public static void iOS_callInt()
    {
        callInt();
    }
}