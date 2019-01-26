#if UNITY_ANDROID
/**
 * Created by zhang on 2019/1/26
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//通信安卓
// jo.Call(method ,parameter );//调用实例方法
// jo.Get(method ,parameter );//获取实例变量(非静态)
// jo.Set(method ,parameter );//设置实例 变量(非静态)
// jo.CallStatic(method ,parameter );//调用静态变量(非静态)
// jo.GetStatic (method ,parameter );//获取静态变量
// jo.SetStatic (method ,parameter );//设置静态变量
public class Define_Android : Sing<Define_Android>
{

    private AndroidJavaObject jo;
    
    public void Init()
    {
        //call包名
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
    //无参 无返回值
    public void CallVoid()
    {
        jo.Call("XXXXXXX");
    }
    //无参 有返回值
    public int CallInt()
    {
        return jo.Call<int>("XXXXXXX");
    }
    //无参 有返回值
    public float CallFloat()
    {
        return jo.Call<float>("XXXXXXX");
    }
    //无参 有返回值
    public float CallString()
    {
        return jo.Call<string>("XXXXXXX");
    }

    //有参 无返回值
    public void CallVoid(string str)
    {
        jo.Call("XXXXXXX",str);
    }
}

#endif
